using DreamSchedulerApplication.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamSchedulerApplication.Controllers
{
    public class StudentController : Controller
    {

        private readonly IGraphClient client;
        public StudentController(IGraphClient graphClient)
        {
            client = graphClient;
        }

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null)
            {
                ViewBag.Message = "The content you are trying to access is for registered users only. Please log in or create new account.";
                filterContext.Result = View("../Account/Login", null); ;
            }
        }

        // GET: Student
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AcademicRecord()
        {

            var academicRecord = new AcademicRecord();

            var student = client.Cypher
                         .Match("(s:Student)")
                         .Where((Student s) => s.StudentID == "123456")
                         .Return((s) => s.As<Student>())
                         .Results.First();

            academicRecord.Student = student;

            academicRecord.CompletedCourses = client.Cypher
                         .Match("(s:Student)-[r:Completed]->(c:Course)")
                         .Where((Student s) => s.StudentID == "123456")
                         .Return((c, r) => new AcademicRecord.CourseEntry
                         {
                             Course = c.As<Course>(),
                             Completed = r.As<Completed>()
                         })
                         .OrderBy("r.semester")
                         .Results;

            return View(academicRecord);
        }

        public ActionResult CourseSequence()
        {
            var courseSequence = new CourseSequence();

            courseSequence.CourseList = client.Cypher
                         .Match("(p:Program)-[r]->(c:Course)")
                         .Return((c, r) => new CourseSequence.CourseEntry
                         {
                             Course = c.As<Course>(),
                             Semester = r.As<ContainsCourse>().SemesterInSequence
                         })
                         .OrderBy("r.SemesterInSequence")
                         .Results;

            return View(courseSequence);
        }
    
    }
}
