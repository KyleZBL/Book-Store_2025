using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginPage.Controllers
{
    public class AccountController : Controller
    {
        // Temporary hardcoded user credentials
        private readonly string enteredUsername = "User1";
        private readonly string enteredPassword = "password123";

        // Displays the login page
        public ActionResult Login()
        {
            return View();
        }

        // Handles POST request for login
        [HttpPost]
        public ActionResult Authenticate(string username, string password)
        {
            if (username == enteredUsername && password == enteredPassword)
            {
                // Redirect to Administrator interface upon successful login
                return RedirectToAction("Administrator", "Book");
            }
            else
            {
                // Failed login attempt, display error message
                ViewBag.Message = "Invalid username or password.";
                return View("Login");
            }
        }
    }
}