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
    }
}