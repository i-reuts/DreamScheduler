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

        private readonly IGraphClient client;
        public HomeController(IGraphClient graphClient)
        {
            client = graphClient;
        }

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
             
    }
}