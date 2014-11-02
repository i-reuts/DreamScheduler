﻿using DreamSchedulerApplication.Models;
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

            var currentStudent =   client.Cypher
                                    .Match("(u:User)-->(s:Student)")
                                    .Where((User u) => u.Username == HttpContext.User.Identity.Name)
                                    .Return((s) => s.As<Student>())
                                    .Results.First();

            academicRecord.Student = currentStudent;

            academicRecord.CompletedCourses = client.Cypher
                         .Match("(s:Student)-[r:Completed]->(c:Course)")
                         .Where((Student s) => s.StudentID == currentStudent.StudentID)
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
                 .Match("(c:Course), (u:User)-->(s:Student)")
                 .Where((Course c) => c.Code == courseEntry.Course.Code)
                 .AndWhere((User u) => u.Username == HttpContext.User.Identity.Name)
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
                         .Match("(u:User)-->(s:Student)-[r:Completed]->(c:Course)")
                         .Where((User u) => u.Username == HttpContext.User.Identity.Name)
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
                         .Match("(u:User)-->(s:Student)-[r:Completed]->(c:Course)")
                         .Where((User u) => u.Username == HttpContext.User.Identity.Name)
                         .AndWhere((Course c) => c.Code == completedCourse.Course.Code)
                         .Set("r = {newRelationship}")
                         .WithParam("newRelationship", completedCourse.Completed)
                         .ExecuteWithoutResults();

                return RedirectToAction("Index");
            }
            return View(completedCourse);
        }

        // GET: Movies/Edit/5
        public ActionResult DeleteCourseEntry(string code)
        {
            if (code == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AcademicRecord.CourseEntry completedCourse = client.Cypher
                         .Match("(u:User)-->(s:Student)-[r:Completed]->(c:Course)")
                         .Where((User u) => u.Username == HttpContext.User.Identity.Name)
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
        [HttpPost, ActionName("DeleteCourseEntry")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteCourse(string code)
        {
            if (ModelState.IsValid)
            {
                client.Cypher
                         .Match("(u:User)-->(s:Student)-[r:Completed]->(c:Course)")
                         .Where((User u) => u.Username == HttpContext.User.Identity.Name)
                         .AndWhere((Course c) => c.Code == code)
                         .Delete("r")
                         .ExecuteWithoutResults();

                return RedirectToAction("Index");
            }
            return View(code);
        }


    }
}