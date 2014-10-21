

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;  

namespace DreamSchedulerApplication.Models
{
    public class LoginTest
    {

        [Required(ErrorMessage = "Login ID is required")]
        [Display(Name="Login ID")]
        public String Username { get; set; }


        [Required(ErrorMessage = "Password is required")]
        [Display(Name="Password")]
        public String Password { get; set; }

    }
}

