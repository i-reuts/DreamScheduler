using DreamSchedulerApplication.Models;
using Neo4jClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace DreamSchedulerApplication.Controllers
{
    [Authorize]
    public class AcademicRecordController : Controller
    {

        private readonly IGraphClient client;
        public AcademicRecordController(IGraphClient graphClient)
        {
            client = graphClient;
        }

        public ActionResult Index()
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

        // GET: Movies/Create
        public ActionResult CreateCourseEntry()
        {
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateCourseEntry(AcademicRecord.CourseEntry courseEntry)
        {
            if (ModelState.IsValid)
            {
                //Add course
                client.Cypher
                .Match("(c:Course), (s:Student)")
                .Where((Course c) => c.Code == courseEntry.Course.Code)
                .AndWhere((Student s) => s.StudentID == "123456")
                .Create("(s)-[r:Completed {completed}]->(c)")
                .WithParam("completed", courseEntry.Completed)
                .ExecuteWithoutResults();

                return RedirectToAction("Index");
            }

            return View(courseEntry);
        }

        // GET: Movies/Edit/5
        public ActionResult EditCourseEntry(string code)
        {
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicRecord.CourseEntry completedCourse = client.Cypher
                         .Match("(s:Student)-[r:Completed]->(c:Course)")
                         .Where((Student s, Course c) => s.StudentID == "123456")
                         .AndWhere((Course c) => c.Code == code)
                         .Return((c, r) => new AcademicRecord.CourseEntry
                         {
                             Completed = r.As<Completed>(),
                             Course = c.As<Course>()
                         })
                         .Results
                         .Single();
            if (completedCourse == null)
            {
                return HttpNotFound();
            }
            return View(completedCourse);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditCourseEntry(AcademicRecord.CourseEntry completedCourse)
        {
            if (ModelState.IsValid)
            {
                client.Cypher
                         .Match("(s:Student)-[r:Completed]->(c:Course)")
                         .Where((Student s, Course c) => s.StudentID == "123456")
                         .AndWhere((Course c) => c.Code == completedCourse.Course.Code)
                         .Set("r = {newRelationship}")
                         .WithParam("newRelationship", completedCourse.Completed)
                         .ExecuteWithoutResults();

                return RedirectToAction("AcademicRecord");
            }
            return View(completedCourse);
        }


    }
}