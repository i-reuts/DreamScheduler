using DreamSchedulerApplication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DreamSchedulerApplication.Controllers
{
    public class AdminController : Controller
    {

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (Session["User"] == null || ((User)Session["User"]).Admin == false)
            {
                ViewBag.Message = "The content you are trying to access is for administrators only. Please log in with your administrator account.";
                filterContext.Result = View("../Account/Login", null); ;
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
    }
}
