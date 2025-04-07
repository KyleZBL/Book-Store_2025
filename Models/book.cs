using System;

namespace Book_Store.Models
{
    public class Book
    {
        public int BookID { get; set; }
        public string Title { get; set; }
        public string ISBN { get; set; }
        public int? AuthorID { get; set; } 
        public string Genre { get; set; }
        public decimal? Price { get; set; }
        public int? Stock { get; set; }    
        public int? Pages { get; set; }     
        public DateTime PublishingDate { get; set; }


        // Parameterless constructor
        public Book() { }

        // Constructor with full properties
        public Book(int bookID, string isbn, string title, int authorID, string genre, decimal price, int stock, int pages, DateTime publishingDate)
        {
            BookID = bookID;
            ISBN = isbn;
            Title = title;
            AuthorID = authorID;
            Genre = genre;
            Price = price;
            Stock = stock;
            Pages = pages;
            PublishingDate = publishingDate;
        }
    }
}