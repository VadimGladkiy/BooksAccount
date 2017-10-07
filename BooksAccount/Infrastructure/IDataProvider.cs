using System;
using System.Collections.Generic;
using BooksAccount.Models;

namespace BooksAccount.Infrastructure
{
    public interface IDataProvider
    {
        void OpenConnection(string _connectionString);
        void CloseConnection();
        int getNumberOfItems();

        IEnumerable <Book> allBooksAsList(int page, int howManyToSkip);
        Book getOne(int book_id);
        void addBook(Book book);
        bool deleteBook(int book_id); 
    }
}
