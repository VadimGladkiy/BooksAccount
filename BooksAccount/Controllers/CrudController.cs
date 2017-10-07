using System;
using System.Configuration;
using System.Web.Http;
using BooksAccount.Models;
using BooksAccount.Infrastructure;

namespace BooksAccount.Controllers
{
    public class CrudController : ApiController
    {
        private string connection_string =
           ConfigurationManager.ConnectionStrings["BooksContext"].ConnectionString;
        private int itemsCount;

        private IDataProvider dataRepository;

        public CrudController(IDataProvider bindedContext)
        {
            dataRepository = bindedContext; 
        }
        [Route("api/Crud/pagesInfo")]
        public IHttpActionResult GetItemsCount() 
        {
            dataRepository.OpenConnection(connection_string);
            itemsCount = dataRepository.getNumberOfItems();
            dataRepository.CloseConnection();
            return Ok(itemsCount);
        }
        
        // GET: api/Crud
        [HttpGet]
        public IHttpActionResult Get(int cPage , int howSkip )
        {
            dataRepository.OpenConnection(connection_string);
            var books = dataRepository.allBooksAsList(cPage,howSkip);
            dataRepository.CloseConnection();
            return Ok(books);
        }

        // GET: api/Crud/5
        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            dataRepository.OpenConnection(connection_string);
            var book = dataRepository.getOne(id);
            dataRepository.CloseConnection();
            if (book == null)
            {
                return NotFound();
            }
            return Ok();
        }

        // POST: api/Crud
        [HttpPost]
        public IHttpActionResult Post([FromBody]Book newInstance)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            dataRepository.OpenConnection(connection_string);
           
            dataRepository.addBook(newInstance);
            dataRepository.CloseConnection();
            String location = Request.RequestUri + "/" + newInstance.book_id.ToString();
            return Created(location,newInstance);
        }

        // PUT: api/Crud/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Crud/5
        [Route("api/Crud/{id}/delete")]
        public IHttpActionResult Delete(int id)
        {
            dataRepository.OpenConnection(connection_string);
            bool result = dataRepository.deleteBook(id);
            dataRepository.CloseConnection();
            if (result)
            {
                return Ok();
            }
            else { return NotFound(); }
        }
    }
}
