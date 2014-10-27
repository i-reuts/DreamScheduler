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

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null)
            {
                ViewBag.popup = "notlogged";
                filterContext.Result = View("../Account/Login", null); ;
            }
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

            var academicRecord = new AcademicRecord();

            var student = client.Cypher
                         .Match("(s:Student)")
                         .Where((Student s) => s.student_id == "123456")
                         .Return((s) => s.As<Student>())
                         .Results.First();

            academicRecord.student = student;

            var coursesCompleted = client.Cypher
                         .Match("(s:Student)-[rel1:completed]->(c:Course)")
                         .Where((Student s) => s.student_id == "123456")
                         .Return((c, rel1) => new 
                         {
                             course =  c.As<Course>(),
                             completed = rel1.As<Completed>()
                         })
                         .Results;

            //academicRecord.completedCourses = coursesCompleted;

            return View();
        }
             
    }
}