using System.Collections.Generic;
using Book_Store.Models;

namespace Book_Store.DataAccess
{
    public interface IDataAccess
    {
        void Create(Book book);
        IEnumerable<Book> GetAll();
        Book GetById(int id);
        void Update(Book book);
        void Delete(int id);
        List<Book> GetBooksByAuthor(string authorName);
        List<Book> GetBooksByGenre(string authorName);
    }
}