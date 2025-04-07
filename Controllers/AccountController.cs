using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginPage.Controllers

// AccountController class for user login 
{
    public class AccountController : Controller
    {
        // variables that hold temperary data for user validation
        private string enteredUsername = "User1";
        private string enteredPassword = "password123";

        public ActionResult Login()
        {
            return View();
        }


        // POST: Account/Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (username == enteredUsername && password == enteredPassword)
            {
                ViewBag.Message = "Login successful!";
                return View();
            }
            else
            {
                // Failed login
                ViewBag.Message = "Invalid username or password.";
                return View();
            }
        }

    }

}

