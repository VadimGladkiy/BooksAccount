using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using BooksAccount.Models;

namespace BooksAccount.Infrastructure
{
    public class DataContext : IDataProvider
    {
        private String sql_expression;

        //for a database access
        private SqlConnection SqlConn;
        
        public void OpenConnection(string _connectionString)
        {
            SqlConn = new SqlConnection();
            SqlConn.ConnectionString = _connectionString;
            SqlConn.Open();       
        }
        public void CloseConnection()
        {
            SqlConn.Close();
        }
        int IDataProvider.getNumberOfItems()
        {
            int result =0;
            sql_expression = "Select Count(book_id) from Books";
            using (SqlCommand select_count_command = new SqlCommand(sql_expression, SqlConn))
            {
                var obj = select_count_command.ExecuteScalar();
                result = (int)obj;
            }
            return result;
        }

        IEnumerable<Book> IDataProvider.allBooksAsList(int page, int howMany)
        {
                sql_expression = String
                .Format(" select * from Books Order by book_id "
                +"offset ({0}*{1}) Rows Fetch next {1} Rows only"
                , page-1 , howMany );
            // select * from Books Order by book_id offset 2 ROWS FETCH NEXT 2 ROWS ONLY;

            return dataReaderBookIteration(sql_expression);
        }
        Book IDataProvider.getOne(int book_id)
        {
            sql_expression = String.Format(" select * from Books "
            + "where book_id = {0}", book_id);
            return dataReaderBookIteration(sql_expression)[0];
        }
        protected List<Book> dataReaderBookIteration(string sql_expression)
        {
            List<Book> list = new List<Book>();
            try
            {
                using (SqlCommand select_command = new SqlCommand(sql_expression, SqlConn ))
                {
                    Book toList;
                    SqlDataReader BooksReader = select_command.ExecuteReader();
                    while (BooksReader.Read())
                    {
                        toList = new Book()
                        {
                            book_id = Int32.Parse(BooksReader["book_id"].ToString()),
                            authors_names = BooksReader["authors_names"].ToString(),
                            book_name = BooksReader["book_name"].ToString(),
                            ISBN = BooksReader["ISBN"].ToString(),
                            book_cost = double.Parse(BooksReader["book_cost"].ToString()),
                            genre = BooksReader["genre"].ToString()                       
                        };
                            list.Add(toList);
                        }
                        BooksReader.Close();
                    }
                }
                catch (Exception e)
                {
                    return null;
                }
                return list;
            }

        void IDataProvider.addBook(Book book)
        {
            sql_expression = string.Format(" insert into Books "
            + "( authors_names, book_name, ISBN, book_cost, genre)" 
            +"values ('{0}','{1}','{2}',{3},'{4}');"
            ,book.authors_names,book.book_name,book.ISBN,book.book_cost,book.genre);       

            using (SqlCommand insert_command = new SqlCommand(sql_expression, SqlConn))
            {
                insert_command.ExecuteNonQuery();
            }
        }

        bool IDataProvider.deleteBook(int book_id)
        {
            sql_expression = string.Format(" delete from Books " +
            "WHERE book_id = {0}", book_id);
            try
            {
                using (SqlCommand delete_command = new SqlCommand(sql_expression, SqlConn))
                {
                    delete_command.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}