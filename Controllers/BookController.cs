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

        public BookController()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
            _dataAccess = new BookDataAccess();
        }

        public ActionResult User_Interface(int? authorId)
        {
            var books = _dataAccess.GetAll();
            if (authorId.HasValue)
            {
                books = books.Where(b => b.AuthorID == authorId.Value).ToList();
            }
            return View("User_Interface", books);
        }

        public ActionResult Administrator()
        {
            var books = _dataAccess.GetAll();
            return View("Administrator_Interface", books);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View("Create");
        }

        [HttpPost]
        public ActionResult Create(Book book, string AuthorFirstName, string AuthorLastName)
        {
            int authorId = book.AuthorID.GetValueOrDefault(0);

            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

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
                            using (var createAuthorCmd = new SqlCommand("INSERT INTO Author (FirstName, LastName) OUTPUT INSERTED.AuthorId VALUES (@FirstName, @LastName)", conn))
                            {
                                createAuthorCmd.Parameters.AddWithValue("@FirstName", AuthorFirstName);
                                createAuthorCmd.Parameters.AddWithValue("@LastName", AuthorLastName);
                                authorId = Convert.ToInt32(createAuthorCmd.ExecuteScalar());
                            }
                        }
                    }
                }

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

        [HttpGet]
        public ActionResult Update(int id)
        {
            var book = _dataAccess.GetById(id);
            if (book == null)
            {
                return HttpNotFound();
            }

            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        SELECT a.FirstName, a.LastName
        FROM Author a
        INNER JOIN Books b ON a.AuthorId = b.AuthorID
        WHERE b.BookID = @BookID;", conn))
            {
                cmd.Parameters.AddWithValue("@BookID", id);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        book.Author = new Author
                        {
                            FirstName = reader["FirstName"].ToString(),
                            LastName = reader["LastName"].ToString()
                        };
                    }
                }
            }

            return View("Update", book);
        }


        [HttpPost]
        public ActionResult Update(Book book)
        {
            if (ModelState.IsValid)
            {
                using (var conn = new SqlConnection(_connectionString))
                {
                    conn.Open();

                    // Update book details
                    using (var cmd = new SqlCommand(@"
                UPDATE Books 
                SET Title = @Title, Genre = @Genre, Price = @Price, Stock = @Stock, 
                    Pages = @Pages, PublishingDate = @PublishingDate
                WHERE BookID = @BookID;", conn))
                    {
                        cmd.Parameters.AddWithValue("@BookID", book.BookID);
                        cmd.Parameters.AddWithValue("@Title", book.Title);
                        cmd.Parameters.AddWithValue("@Genre", book.Genre);
                        cmd.Parameters.AddWithValue("@Price", book.Price);
                        cmd.Parameters.AddWithValue("@Stock", book.Stock);
                        cmd.Parameters.AddWithValue("@Pages", book.Pages);

                        DateTime validDate = book.PublishingDate < new DateTime(1753, 1, 1) ? new DateTime(1753, 1, 1) : book.PublishingDate;
                        cmd.Parameters.AddWithValue("@PublishingDate", validDate);

                        cmd.ExecuteNonQuery();
                    }

                    // Only update the author's name IF it has actually changed
                    using (var authorCmd = new SqlCommand(@"
                      UPDATE Author
                            SET FirstName = @FirstName, LastName = @LastName
                            WHERE AuthorId = (SELECT AuthorID FROM Books WHERE BookID = @BookID)
                            AND AuthorId = @AuthorID;", conn))
                    {
                        authorCmd.Parameters.AddWithValue("@BookID", book.BookID); 
                        authorCmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
                        authorCmd.Parameters.AddWithValue("@FirstName", book.Author.FirstName);
                        authorCmd.Parameters.AddWithValue("@LastName", book.Author.LastName);
                        authorCmd.ExecuteNonQuery();
                    }
                }

                return RedirectToAction("Administrator");
            }

            return View("Update", book);
        }

        [HttpPost]
        public ActionResult Delete(int book_Id)
        {
            _dataAccess.Delete(book_Id);
            return RedirectToAction(nameof(User_Interface));
        }
    }
}