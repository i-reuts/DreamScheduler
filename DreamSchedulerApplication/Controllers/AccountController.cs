using Neo4jClient;
using System;
using System.Linq;
using System.Web.Mvc;
using DreamSchedulerApplication.Models;
using DreamSchedulerApplication.Libraries;

namespace DreamSchedulerApplication.Controllers
{
    public class AccountController : Controller
    {

        private readonly IGraphClient client;

        public AccountController(IGraphClient graphClient)
        {
            client = graphClient;
        }

        private User user;

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //store input in strings
                user = new User { Username = model.Username, Password = model.Password };

                var query = client
                                  .Cypher
                                  .Match("(n:Account)")
                                  .Where(((LoginViewModel n) => n.Username == user.Username))
                                  .Return(n => n.As<LoginViewModel>())
                                  .Results;

                try
                {
                    var testing = query.Single();
                    if(PasswordHash.ValidatePassword(model.Password,testing.Password))
                    {
                        //if user is logged in, create session
                        Session["User"] = user.Username;
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ViewBag.errorPassword = "password is wrong, try again";
                        return View("login");
                    }
                } //if we can't find the account = it doest not exist, neo4j will cause a error 
                catch (InvalidOperationException)
                {
                    //error invalid user account 
                    
                    ViewBag.errorUsername = "this user does not exist ";
                    return View("login");
                }
            }

            // model is not valid
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var encryptedPassword = PasswordHash.CreateHash(model.Password);
                var newAccount = new User { Username = model.Username, Password = encryptedPassword };

                var newStudent = new Student { first_name = model.first_name, last_name = model.last_name, student_id = model.student_id, GPA = model.GPA };

                // create the account in the database
                client.Cypher
                             .Create("(u:User {newAcount})-[:is_a]->(s:Student {newStudent})")
                             .WithParam("newAcount", newAccount)
                             .WithParam("newStudent", newStudent)
                             .ExecuteWithoutResults();

                return RedirectToAction("About", "Home");
            }

            // model is not valid
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Login", "Account");
        }

    }
}