using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Configuration;
using Book_Store.Models;
using System.Diagnostics;

namespace Book_Store.DataAccess
{
    public class BookDataAccess : IDataAccess
    {
        private readonly string _connectionString;

        public BookDataAccess()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["connString"].ConnectionString;

            // Debugging: Print the connection string to verify it's being loaded
            Console.WriteLine("Connection String Loaded: " + _connectionString);

        }

        public void Create(Book book)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("INSERT INTO Books (Title, AuthorID, Genre, Price, Stock, Pages, PublishingDate) VALUES (@Title, @AuthorID, @Genre, @Price, @Stock, @Pages, @PublishingDate)", conn))
            {
                cmd.Parameters.AddWithValue("@Title", book.Title);
                cmd.Parameters.AddWithValue("@AuthorID", book.AuthorID);
                cmd.Parameters.AddWithValue("@Genre", book.Genre);
                cmd.Parameters.AddWithValue("@Price", book.Price);
                cmd.Parameters.AddWithValue("@Stock", book.Stock);
                cmd.Parameters.AddWithValue("@Pages", book.Pages);
                cmd.Parameters.AddWithValue("@PublishingDate", book.PublishingDate);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public IEnumerable<Book> GetAll()
        {
            var books = new List<Book>();
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM Books", conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        books.Add(new Book
                        {
                            BookID = (int)reader["BookID"],
                            Title = (string)reader["Title"],
                            AuthorID = (int)reader["AuthorID"],
                            Genre = (string)reader["Genre"],
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            Pages = (int)reader["Pages"],
                            PublishingDate = (DateTime)reader["PublishingDate"]
                        });
                    }
                }
            }
            return books;
        }

        public Book GetById(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("SELECT * FROM Books WHERE BookID = @BookID", conn))
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
                            Title = (string)reader["Title"],
                            AuthorID = (int)reader["AuthorID"],
                            Genre = (string)reader["Genre"],
                            Price = (decimal)reader["Price"],
                            Stock = (int)reader["Stock"],
                            Pages = (int)reader["Pages"],
                            PublishingDate = (DateTime)reader["PublishingDate"]
                        };
                    }
                }
            }
            return null;
        }

        public void Update(Book book)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Build update query 
                var query = "UPDATE Books SET ";
                var parameters = new List<SqlParameter>();

                if (!string.IsNullOrEmpty(book.Title))
                {
                    query += "Title = @Title, ";
                    parameters.Add(new SqlParameter("@Title", book.Title));
                }
                if (!string.IsNullOrEmpty(book.ISBN))
                {
                    query += "ISBN = @ISBN, ";
                    parameters.Add(new SqlParameter("@ISBN", book.ISBN));
                }
                if (!string.IsNullOrEmpty(book.Genre))
                {
                    query += "Genre = @Genre, ";
                    parameters.Add(new SqlParameter("@Genre", book.Genre));
                }
                if (book.Price.HasValue)
                {
                    query += "Price = @Price, ";
                    parameters.Add(new SqlParameter("@Price", book.Price));
                }
                if (book.Stock.HasValue)
                {
                    query += "Stock = @Stock, ";
                    parameters.Add(new SqlParameter("@Stock", book.Stock));
                }
                if (book.Pages.HasValue)
                {
                    query += "Pages = @Pages, ";
                    parameters.Add(new SqlParameter("@Pages", book.Pages));
                }
                if (book.PublishingDate != default)
                {
                    query += "PublishingDate = @PublishingDate, ";
                    parameters.Add(new SqlParameter("@PublishingDate", book.PublishingDate));
                }

                // Remove last comma and adds WHERE clause to query
                query = query.TrimEnd(',', ' ') + " WHERE BookID = @BookID";
                parameters.Add(new SqlParameter("@BookID", book.BookID));

               
            }
        }

        public void Delete(int id)
        {
            using (var conn = new SqlConnection(_connectionString))
            using (var cmd = new SqlCommand("DELETE FROM Books WHERE BookID = @BookID", conn))
            {
                cmd.Parameters.AddWithValue("@BookID", id);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}