using System;
using System.ComponentModel.DataAnnotations; 

namespace DreamSchedulerApplication.Models
{
 
        public class User
        {
            public String Username { get; set; }
            public String Password { get; set; }
        }

        public class LoginViewModel
        {
            [Required]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
        }

        public class RegisterViewModel
        {
            [Required(ErrorMessage = "Username is required")]
            [Display(Name = "Username")]
            public string Username { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required(ErrorMessage = "Your first name is required")]
            [Display(Name = "First Name")]
            public string first_name { get; set; }

            [Required(ErrorMessage = "Your last name is required")]
            [Display(Name = "Last Name")]
            public string last_name { get; set; }

            [Required(ErrorMessage = "Your student ID is required")]
            [Display(Name = "Student ID")]
            public string student_id { get; set; }

            [Required(ErrorMessage = "Your GPA is required")]
            [Display(Name = "GPA")]
            public string GPA { get; set; }
        }
}