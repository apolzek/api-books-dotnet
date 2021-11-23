﻿#region snippet_BookServiceClass
using BooksApi.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

namespace BooksApi.Services
{
    public class BookService
    {
        private readonly IMongoCollection<Book> _books;

        #region snippet_BookServiceConstructor
        public BookService(IBookstoreDatabaseSettings settings)
        {
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _books = database.GetCollection<Book>(settings.BooksCollectionName);
        }
        #endregion

        public List<Book> Get() =>
            _books.Find(book => true).ToList();

        public Book Get(string id) =>
            _books.Find<Book>(book => book.Id == id).FirstOrDefault();

        public Book Create(Book book)
        {
            book.Id = "";
            _books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn)
        {
            bookIn.Id = id;
            _books.ReplaceOne(book => book.Id == id, bookIn);
        }

        public void Remove(Book bookIn) =>
            _books.DeleteOne(book => book.Id == bookIn.Id);

        public void Remove(string id) =>
            _books.DeleteOne(book => book.Id == id);

        public void Remove()
        {
            _books.DeleteMany(book => book.Author is string);
        }
    }
}
#endregion
