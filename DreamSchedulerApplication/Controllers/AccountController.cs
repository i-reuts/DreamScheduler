using Neo4jClient;
using System;
using System.Linq;
using System.Web.Mvc;
using DreamSchedulerApplication.Models;
using DreamSchedulerApplication.Libraries;

namespace DreamSchedulerApplication.Controllers
{
    public class AccountController : Controller
    {

        private readonly IGraphClient client;

        public AccountController(IGraphClient graphClient)
        {
            client = graphClient;
        }

        //
        // GET: /Account/Login
        public ActionResult Login()
        {
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User();

                try
                {
                    //Find user account
                    //If not found, neo4j will throw an exception 
                    user = client
                                  .Cypher
                                  .Match("(n:User)")
                                  .Where(((User n) => n.Username == model.Username))
                                  .Return(n => n.As<User>())
                                  .Results.Single();
                }
                catch (InvalidOperationException)
                {
                    //If account not found, display invalid user account error 
                    ViewBag.Message = "A user with this username does not exist";
                    return View("login");
                }

                if(PasswordHash.ValidatePassword(model.Password,user.Password))
                {
                        //if user is logged in, create session
                        Session["User"] = user;
                        if (user.Admin) return RedirectToAction("Index", "Admin");
                        return RedirectToAction("Index", "Student");
                    }
                    else
                    {
                        ViewBag.Message = "Wrong password, please try again";
                        return View("login");
                    }
            }

            // model is not valid
            return View(model);
        }

        //
        // GET: /Account/Register
        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var encryptedPassword = PasswordHash.CreateHash(model.Password);
                var newUser= new User { Username = model.Username, Password = encryptedPassword };

                var newStudent = new Student { FirstName = model.FirstName, LastName = model.LastName, StudentID = model.StudentID, GPA = model.GPA };

                // create the account in the database
                try
                {
                    client.Cypher
                                .Create("(u:User {newAccount})-[:IsA]->(s:Student {newStudent})")
                                .WithParam("newAccount", newUser)
                                .WithParam("newStudent", newStudent)
                                .ExecuteWithoutResults();
                }
                catch (Neo4jClient.NeoException exception)
                {
                    if (exception.Message.Contains("Username")) { ViewBag.Message = "User with such username already exists"; return View("Register"); }
                    else if (exception.Message.Contains("StudentID")) { ViewBag.Message = "Student with such student ID number already exists"; return View("Register"); }
                    else throw exception;
                }

                //Create new session
                Session["User"] = newUser;
                return RedirectToAction("Index", "Student");
            }

            // model is not valid
            return View(model);
        }


        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            Session.Clear();
            return RedirectToAction("Index", "Home");
        }

    }
}