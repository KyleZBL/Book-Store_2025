using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginPage.Controllers

// This is the AccountController class that handles user login and password reset functionality.
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
                // Successful login
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

        // GET: Account/ForgotPassword
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }

        // POST: Account/ForgotPassword
        [HttpPost]
        public ActionResult ForgotPassword(string email, string newPassword)
        {
            // Logic to reset the password and send email
            ResetUserPassword(email, newPassword);

            ViewBag.Message = "Password reset successfully. Please check your email.";
            return View();
        }


        private bool IsValidUser(string username, string password)
        {
            return true;
        }

        private void ResetUserPassword(string email, string newPassword)
        {

        }
    }

}

