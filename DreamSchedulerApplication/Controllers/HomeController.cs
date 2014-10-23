using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Neo4jClient;
using DreamSchedulerApplication.Models;

namespace DreamSchedulerApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult StudentRecord()
        {
            
            //if user is  logged in 
            if(Session["User"] != null)
            {
                return View();
            }
            else
            {
                ViewBag.popup = "notlogged";
                return View("index");
            }
        }
        
        public ActionResult Logout()
        {
            Session.Clear();
            return Redirect("index");

        }

        [HttpPost]
        public ActionResult LoginValidation(LoginTest test)
        {
            if (ModelState.IsValid)
            {
                //store input in strings
                string username = test.Username;
                string password = test.Password;

                GraphClient client = new GraphClient(new Uri("http://localhost:7474/db/data"));
                client.Connect();

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
                        return RedirectToAction("About");
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

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateValidation(LoginTest test)
        {
            if (ModelState.IsValid)
            {
                // save to db, for instance
                GraphClient client = new GraphClient(new Uri("http://localhost:7474/db/data"));
                client.Connect();

                //create newaccount object with input data
                var newAccount = new LoginTest { Username = test.Username, Password = test.Password };

                // create the account in the database
                client.Cypher
                    .Create("(Account:Account {newAcount})")
                    .WithParam("newAcount", newAccount)//against cypher-injection 
                    .ExecuteWithoutResults();
                return RedirectToAction("About");
            }
            // model is not valid
            return View("Create", test);
        }


        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        //DEMO FOR CONNECTION STORE/RETRIEVE DATABASE
        public ActionResult Database()   //if you want to test this out  just do http://localhost:6437/home/Database   
        {
            GraphClient client = new GraphClient(new Uri("http://localhost:7474/db/data"));
            client.Connect();

            var newUser = new User { Id = 123, Name = "Jim" };
            // create the user in the database
            client.Cypher
                .Create("(user:User {newUser})")
                .WithParam("newUser", newUser)//against cypher-injection 
                .ExecuteWithoutResults();

            //find user in database
            var query = client
                .Cypher
                .Match("(user:User)")
                .Where((User user) => user.Id == 456)
                .Return(user => user.As<User>());
            var result = query.Results.Single();
            ViewBag.name = result.Name; //viewbag is used to send info to view 
            ViewBag.id = result.Id;


            return View();
        }

        public ActionResult Login()
        {
            return View();
        }





    }
}