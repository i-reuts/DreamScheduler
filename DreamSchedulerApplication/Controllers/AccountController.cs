using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DreamSchedulerApplication.Models;

namespace DreamSchedulerApplication.Controllers
{
    public class AccountController : Controller
    {
        private readonly IGraphClient client;

        public AccountController(IGraphClient graphClient)
        {
            client = graphClient;
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            return RedirectToAction("index", "Home");

        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult LoginValidation(LoginTest test)
        {
            if (ModelState.IsValid)
            {
                //store input in strings
                string username = test.Username;
                string password = test.Password;

                var query = client
                    .Cypher
                    .Match("(n:Account)")
                    .Where(((LoginTest n) => n.Username == username))
                     .Return(n => n.As<LoginTest>())
                        .Results;

                try
                {
                    var testing = query.Single();
                    if (testing.Password == test.Password)
                    {
                        //if user is logged in, create session
                        Session["User"] = query.Single().Username;
                        return RedirectToAction("About", "Home");
                    }
                    else
                    {
                        ViewBag.errorPassword = "password is wrong, try again";
                        return View("login");
                    }
                }//if we can't find the account = it doest not exist, neo4j will cause a error 
                catch (InvalidOperationException)
                {
                    //error invalid user account 
                    ViewBag.errorUsername = "this user does not exist ";
                    return View("login");
                }
            }
            // model is not valid
            return View("login", test);

        }


        [HttpPost]
        public ActionResult CreateValidation(LoginTest test)
        {
            if (ModelState.IsValid)
            {
                // save to db, for instance

                //create newaccount object with input data
                var newAccount = new LoginTest { Username = test.Username, Password = test.Password };

                // create the account in the database
                client.Cypher
                    .Create("(Account:Account {newAcount})")
                    .WithParam("newAcount", newAccount)//against cypher-injection 
                    .ExecuteWithoutResults();
                return RedirectToAction("About", "Home");
            }
            // model is not valid
            return View("Create", test);
        }
        

    }
}