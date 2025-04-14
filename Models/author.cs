using System;
using System.Collections.Generic;

namespace Book_Store.Models
{
    public class Author
    {
        public int AuthorID { get; set; }
        public string FirstName { get; set;}
        public string LastName { get; set; }

        // Navigation property for books the author has written
        public List<Book> Books { get; set; } = new List<Book>();

        // Default constructor
        public Author() { }

        // Constructor with full properties
        public Author(int authorID, string firstName, string lastName)
        {
            AuthorID = authorID;
            FirstName = firstName;
            LastName = lastName;
        }
    }
   }