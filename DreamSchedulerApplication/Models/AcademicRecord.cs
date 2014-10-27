using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamSchedulerApplication.Models
{
    public class AcademicRecord
    {
        public Student student;

        public IEnumerable<CourseEntry> completedCourses;

        public class CourseEntry {
            public Course course;
            public Completed completed;
        }
    }
}