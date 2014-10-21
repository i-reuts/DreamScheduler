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
                //find user in database
                try
                {
                    var query = client
                    .Cypher
                    .Match("(user:Account)")
                    .Where((LoginTest account) => account.Password == password && account.Username == username)
                    .Return(account => account.As<LoginTest>())
                    .Results;
                }
                    //fuckkkkkkkkkkkkkkkkkkkkkk catch this shittttt if user not found catch error  NEED TO FIX this 
                catch (Neo4jClient.NeoException error )
                {
                    if (error.NeoFullName == "org.neo4j.cypher.SyntaxException")
                    {
                        return View("login");
                    }
                    if( error.InnerException ==null)
                    {
                        return View("login");
                    }
                    if(error.HelpLink == null)
                    {
                        return View("login");
                    }
                }
                
                // save to db, for instance
                return RedirectToAction("Contact");
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
                    .Create("(user:Account {newAcount})")
                    .WithParam("newAcount", newAccount)//against cypher-injection 
                    .ExecuteWithoutResults();
                return RedirectToAction("About");
            }
            // model is not valid
            return View("Create", test);
        }
    }
}