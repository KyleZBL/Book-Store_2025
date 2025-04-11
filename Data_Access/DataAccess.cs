using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Book_Store.Models;

namespace Book_Store.DataAccess
{
    public class BookDataAccess : IDataAccess
    {
        // Database connection string
        private readonly string _connectionString;

        public BookDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;
        }

        public void Create(Book book)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
        INSERT INTO Books (Title, Genre, Price, Stock, Pages, PublishingDate, AuthorID) 
        VALUES (@Title, @Genre, @Price, @Stock, @Pages, @PublishingDate, @AuthorID)", conn))
            {
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Pages", book.Pages);
                cmd.Parameters.AddWithValue("@PublishingDate", book.PublishingDate);
                cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID == 0 ? DBNull.Value : (object)book.AuthorID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Method to get all books with author names
        public IEnumerable<Book> GetAll()
        {
            var books = new List<Book>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT 
                    b.BookID, 
                    b.Title, 
                    b.Genre, 
                    b.Price, 
                    b.Stock, 
                    b.Pages, 
                    b.PublishingDate, 
                    COALESCE(a.FirstName + ' ' + a.LastName, 'No Author Assigned') AS AuthorName
                FROM Books b
                LEFT JOIN Author a ON b.AuthorID = a.AuthorId;", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookID = (int)reader["BookID"],
                            Title = reader["Title"].ToString(),
                            Genre = reader["Genre"].ToString(),
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            Pages = (int)reader["Pages"],
                            PublishingDate = (DateTime)reader["PublishingDate"],
                            AuthorName = reader["AuthorName"].ToString() // Correctly displays the author's full name
                        });
                    }
                }
            }
            return books;
        }

        // Method to get a book by ID
        public Book GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                SELECT 
                    b.BookID, 
                    b.Title, 
                    b.Genre, 
                    b.Price, 
                    b.Stock, 
                    b.Pages, 
                    b.PublishingDate, 
                    COALESCE(a.FirstName + ' ' + a.LastName, 'No Author Assigned') AS AuthorName
                FROM Books b
                LEFT JOIN Author a ON b.AuthorID = a.AuthorId;", conn))
            {
                cmd.Parameters.AddWithValue("@BookID", id);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Book
                        {
                            BookID = (int)reader["BookID"],
                            Title = reader["Title"].ToString(),
                            Genre = reader["Genre"].ToString(),
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            Pages = (int)reader["Pages"],
                            PublishingDate = (DateTime)reader["PublishingDate"],
                            AuthorName = reader["AuthorName"].ToString()
                        };
                    }
                }
            }
            return null;
        }

        // Method to update a book
        public void Update(Book book)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand(@"
                UPDATE Books 
                SET Title = @Title, 
                    Genre = @Genre, 
                    Price = @Price, 
                    Stock = @Stock, 
                    Pages = @Pages, 
                    PublishingDate = @PublishingDate 
                WHERE BookID = @BookID;", conn))
            {
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Pages", book.Pages);
                cmd.Parameters.AddWithValue("@PublishingDate", book.PublishingDate);
                cmd.Parameters.AddWithValue("@BookID", book.BookID);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // Method to delete a book
        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Books WHERE BookID = @BookID;", conn))
            {
                cmd.Parameters.AddWithValue("@BookID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}