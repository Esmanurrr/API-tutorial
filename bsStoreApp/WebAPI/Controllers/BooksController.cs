using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;
using Services.Contracts;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IServiceManager _manager;
        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookService.GetAllBooks(false);
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
                var book = _manager.BookService.GetOneBookById(id, false);
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

                _manager.BookService.CreateOneBook(book);

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

                //check book
                if (book is null)
                    return BadRequest();//404 

                //var entity = _manager.BookService.GetOneBookById(id, true);//selected book
                //we check this in bookmanager class so we dont need to do again, 
                
                _manager.BookService.UpdateOneBook(id, book, true);
                //also we dont have to save this because we already done it in bookmanager
                return NoContent();
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
                /*var entity = _manager.BookService.GetOneBookById(id,false);
                if(entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });//404
                we did this check in bookmanager*/
                _manager.BookService.DeleteOneBook(id, false);
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
                var entity = _manager.BookService.GetOneBookById(id,true);

                if (entity is null)
                    return NotFound();

                bookPatch.ApplyTo(entity);
                _manager.BookService.UpdateOneBook(id, entity, true);
                return NoContent();        

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
