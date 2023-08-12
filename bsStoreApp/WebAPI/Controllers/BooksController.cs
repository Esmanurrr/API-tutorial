using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Models;
using WebAPI.Repositories;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly RepositoryContext _context;
        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                if (!books.Any()) // check books
                {
                    return NotFound("No books found"); // if its null, return 404
                }
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
                var book = _context.Books.Where(x => x.Id == id).SingleOrDefault();
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

                _context.Books.Add(book);
                _context.SaveChanges();

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
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();//selected book

                //check book
                if (book is null)
                    return NotFound();//404 
                //check id
                if (id!=book.Id)
                    return BadRequest();//400


                entity.Title = book.Title;
                entity.Price = book.Price;//we will use mapper then
                //_context.Books.Update(book); it is also correct
                _context.SaveChanges();

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
                var entity = _context.Books.Find(id);
                if(entity is null)
                    return NotFound(new
                    {
                        statusCode = 404,
                        message = $"Book with id:{id} could not found."
                    });//404

                _context.Books.Remove(entity);
                _context.SaveChanges();
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
                var entity = _context.Books.Where(b => b.Id.Equals(id)).SingleOrDefault();

                if (entity is null)
                    return NotFound();

                bookPatch.ApplyTo(entity);
                _context.SaveChanges();
                return NoContent();        

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
