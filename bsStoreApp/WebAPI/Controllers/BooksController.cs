using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryManager _manager;
        public BooksController(RepositoryManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.Book.GetAllBooks(false);
                return Ok(books);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")]int id)
        {
            try
            {
                var book = _manager.Book.GetOneBookById(id, false);
                if (book is null)
                    return NotFound();

                return Ok(book);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            try
            {
                if(book is null)
                    return BadRequest();//400

                _manager.Book.CreateOneBook(book);
                _manager.Save();

                return StatusCode(201,book);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute] int id,[FromBody] Book book)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id, true);//selected book

                //check book
                if (book is null)
                    return NotFound();//404 
                //check id
                if (id!=book.Id)
                    return BadRequest();//400


                entity.Title = book.Title;
                entity.Price = book.Price;//we will use mapper then
                //_context.Books.Update(book); it is also correct
                _manager.Save();

                return Ok(book);
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteOneBook([FromRoute]int id)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id,false);
                if(entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });//404

                _manager.Book.Delete(entity);
                _manager.Save();
                return StatusCode(204);

            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook([FromRoute(Name="id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            try
            {
                var entity = _manager.Book.GetOneBookById(id,true);

                if (entity is null)
                    return NotFound();

                bookPatch.ApplyTo(entity);
                _manager.Book.Update(entity);
                _manager.Save();
                return NoContent();        

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
