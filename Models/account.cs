using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace Book_Store.Models
{
    public class Account
    {
        public int AccountId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool IsAuthor { get; set; }

        // Constructor
        public Account(int accountId, string username, string password, bool isAuthor)
        {
            AccountId = accountId;
            Username = username;
            Password = password;
            IsAuthor = isAuthor;
        }

        // User Validation
        public bool IsValidUser(string inputUsername, string inputPassword)
        {
            return Username == inputUsername && Password == inputPassword;
        }

  
    }
}