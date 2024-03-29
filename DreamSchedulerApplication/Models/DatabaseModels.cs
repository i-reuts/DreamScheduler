﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DreamSchedulerApplication.Models
{
    public class User
    {
        public String Username { get; set; }
        public String Password { get; set; }

        public string Roles { get; set; }
    }

    public class Student
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string StudentID { get; set; }
        public string GPA { get; set; }
    }

    public class Course
    {
        [Display(Name = "Course Code")]
        public string Code { get; set; }

        public string Title { get; set; }
        public string Credits { get; set; }
    }

    public class Completed
    {
        public string Grade { get; set; }
        [Display(Name = "Semester Completed")]
        public int Semester { get; set; }
    }

    public class ContainsCourse
    {
        public int SemesterInSequence { get; set; }
    }
}