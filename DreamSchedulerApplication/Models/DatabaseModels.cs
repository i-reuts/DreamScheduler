using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DreamSchedulerApplication.Models
{
    public class Student
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string student_id { get; set; }
        public string GPA { get; set; }
    }

    public class Course
    {
        public string code { get; set; }
        public string title { get; set; }
        public string credits { get; set; }
    }

    public class Completed
    {
        public string grade { get; set; }
        public int semester { get; set; }
    }
}