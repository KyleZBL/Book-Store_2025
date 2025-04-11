using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Book_Store.Models;
using Book_Store.DataAccess;
using System.Data.SqlClient;
using System.Configuration;

namespace Book_Store.Controllers
{
    public class BookController : Controller
    {
        private readonly string _connectionString;
        private readonly IDataAccess _dataAccess;

        // Constructor
        public BookController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            _dataAccess = new BookDataAccess();
        }
        //function to display the user interface
        // Display all books or filter by Author ID
        public ActionResult User_Interface(int? authorId)
        {
            var books = _dataAccess.GetAll();
            if (authorId.HasValue)
            {
                books = books.Where(b => b.AuthorID == authorId.Value).ToList();
            }
            return View("User_Interface", books);
        }

        // function to display the administrator interface
        public ActionResult Administrator()
        {
            var books = _dataAccess.GetAll();
            return View("Administrator_Interface", books);
        }

        // Create //

        //Returns the view for creating a new book
        [HttpGet]
        public ActionResult Create()
        {
            return View("Create"); // Ensures the view loads
        }

        // Create a new book entry
        [HttpPost]
        public ActionResult Create(Book book, string AuthorFirstName, string AuthorLastName)
        {
            int authorId = book.AuthorID.GetValueOrDefault(0);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Check if the author exists
                if (authorId == 0 && !string.IsNullOrEmpty(AuthorFirstName) && !string.IsNullOrEmpty(AuthorLastName))
                {
                    using (var checkCmd = new SqlCommand("SELECT AuthorId FROM Author WHERE FirstName = @FirstName AND LastName = @LastName", conn))
                    {
                        checkCmd.Parameters.AddWithValue("@FirstName", AuthorFirstName);
                        checkCmd.Parameters.AddWithValue("@LastName", AuthorLastName);

                        var existingAuthor = checkCmd.ExecuteScalar();
                        if (existingAuthor != null)
                        {
                            authorId = Convert.ToInt32(existingAuthor);
                        }
                        else
                        {  
                            // Create new author
                            using (var createAuthorCmd = new SqlCommand("INSERT INTO Author (FirstName, LastName) OUTPUT INSERTED.AuthorId VALUES (@FirstName, @LastName)", conn))
                            {
                                createAuthorCmd.Parameters.AddWithValue("@FirstName", AuthorFirstName);
                                createAuthorCmd.Parameters.AddWithValue("@LastName", AuthorLastName);
                                authorId = Convert.ToInt32(createAuthorCmd.ExecuteScalar());
                            }
                        }
                    }
                }
                // Insert the book with the author ID
                using (var cmd = new SqlCommand("INSERT INTO Books (Title, Genre, Price, Stock, Pages, PublishingDate, AuthorID) VALUES (@Title, @Genre, @Price, @Stock, @Pages, @PublishingDate, @AuthorID)", conn))
                {
                    cmd.Parameters.AddWithValue("@Title", book.Title);
                    cmd.Parameters.AddWithValue("@Genre", book.Genre);
                    cmd.Parameters.AddWithValue("@Price", book.Price);
                    cmd.Parameters.AddWithValue("@Stock", book.Stock);
                    cmd.Parameters.AddWithValue("@Pages", book.Pages);
                    cmd.Parameters.AddWithValue("@PublishingDate", book.PublishingDate);
                    cmd.Parameters.AddWithValue("@AuthorID", authorId);

                    cmd.ExecuteNonQuery();
                }
            }

            return RedirectToAction("Administrator");
        }

        // Update book details
        [HttpPost]
        public ActionResult Update(Book book)
        {
            if (ModelState.IsValid)
            {
                _dataAccess.Update(book);
                return RedirectToAction(nameof(User_Interface));
            }
            return View("User_Interface");
        }

        // Delete book
        [HttpPost]
        public ActionResult Delete(int book_Id)
        {
            _dataAccess.Delete(book_Id);
            return RedirectToAction(nameof(User_Interface));
        }
    }
}