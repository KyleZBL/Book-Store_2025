using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Book_Store.Models;
using Book_Store.DataAccess;

namespace Book_Store.Controllers
{
    public class BookController : Controller
    {
        private readonly IDataAccess _dataAccess;

        // Constructor
        public BookController()
        {
            _dataAccess = new BookDataAccess();
        }

        // Display all books or filter by Author ID
        public ActionResult User_Interface(int? authorId)
        {
            var books = _dataAccess.GetAll();

            // Filter books by Author ID if provided
            if (authorId.HasValue)
            {
                books = books.Where(b => b.AuthorID == authorId.Value).ToList();
            }

            return View("User_Interface", books);
        }


        public ActionResult Administrator()
        {
            var books = _dataAccess.GetAll();
            return View("Administrator_Interface", books); // Matches the file name!
        }

        // Create new book entry
        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _dataAccess.Create(book);
                return RedirectToAction(nameof(User_Interface));
            }
            return View("User_Interface");
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