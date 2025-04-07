using System.Collections.Generic;
using System.Web.Mvc;
using Book_Store.Models;
using Book_Store.DataAccess;

namespace Book_Store.Controllers
{
    public class BookController : Controller
    {
        private readonly IDataAccess _dataAccess;
        public BookController()
        {
            _dataAccess = new BookDataAccess(); // Instantiating the data access layer
        }

        // Display all books
        public ActionResult User_Interface()
        {
            var books = _dataAccess.GetAll(); 
            return View("User_Interface", books);
        }

        // Create new book entry
        [HttpPost]
        public ActionResult Create(Book book)
        {
            if (ModelState.IsValid)
            {
                _dataAccess.Create(book); 
                return RedirectToAction("User_Interface");
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
                return RedirectToAction("User_Interface");
            }
            return View("User_Interface");
        }

        // Delete book
        [HttpPost]
        public ActionResult Delete(int book_Id)
        {
            _dataAccess.Delete(book_Id);
            return RedirectToAction("User_Interface");
        }
    }
}