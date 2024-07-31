using Entities.DataTransferObject;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;


namespace Presentation.Controllers
{
    [Route("api/books")]
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
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBooks([FromRoute(Name = "id")] int id)
        {
            var book = _manager.BookService.GetOneBookById(id, false);
            return Ok(book);
        }


        [HttpPost]
        public IActionResult CreateOneBook([FromBody] Book book)
        {
            if (book == null)
                return BadRequest();

            _manager.BookService.CreateOneBook(book);

            return StatusCode(201, book);
        }


        [HttpPut("{id:int}")]
        public IActionResult UpdateOneBook([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            if (bookDto is null)
                return BadRequest();

            _manager.BookService.UpdateOneBook(id, bookDto, true);

            return NoContent();
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeletOneBooks([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.DeleteOneBook(id, false);
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateOneBook(
            [FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<Book> bookPatch)
        {
            var entity = _manager.BookService.GetOneBookById(id, true);

            bookPatch.ApplyTo(entity);
            _manager.BookService.UpdateOneBook(id, new BookDtoForUpdate(entity.Id, entity.Title, entity.Price), true);
            return NoContent();

        }


        //[HttpGet]
        //public IActionResult GetAllBooks()
        //{
        //    var books = ApplicationContext.Books;
        //    return Ok(books);
        //}

        //[HttpGet("{id:int}")]
        //public IActionResult GetOneBooks([FromRoute(Name ="id")] int id)
        //{
        //    var book = ApplicationContext
        //        .Books
        //        .Where(x => x.Id.Equals(id))
        //        .SingleOrDefault(); //tek bir kayıt yada null don

        //    if(book == null)
        //        return NotFound();
        //    else
        //        return Ok(book);
        //}


        //[HttpPost]
        //public IActionResult CreateOneBook([FromBody] Book book)
        //{
        //    try
        //    {
        //        if (book == null)
        //            return BadRequest();

        //        ApplicationContext.Books.Add(book);
        //        return StatusCode(201, book);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}



        //[HttpPut("{id:int}")]
        //public IActionResult UpdateOneBook([FromRoute(Name ="id")] int id, [FromBody] Book book)
        //{
        //    //check Book?
        //    var entity = ApplicationContext
        //        .Books
        //        .Find(x => x.Id.Equals(id));

        //    if (entity == null)
        //        return NotFound();

        //    //check Id?
        //    if(id != book.Id)
        //        return BadRequest();

        //    ApplicationContext.Books.Remove(entity);
        //  //  book.Id = entity.Id;
        //    ApplicationContext.Books.Add(book);
        //    return Ok(book);
        //}


        //[HttpDelete]
        //public IActionResult DeletAllBooks()
        //{
        //    ApplicationContext.Books.Clear();
        //    return NoContent();
        //}


        //[HttpDelete("{id:int}")]
        //public IActionResult DeletOneBooks([FromRoute(Name = "id")] int id)
        //{
        //    //check Book?
        //    var entity = ApplicationContext
        //        .Books
        //        .Find(x => x.Id.Equals(id));

        //    if (entity is null)
        //        return NotFound(new
        //        {
        //            statusCode = 404,
        //            message = $"Book id{id} not found"
        //        });

        //    ApplicationContext.Books.Remove(entity);
        //    return NoContent();
        //}


        //[HttpPatch("{id:int}")]
        //public IActionResult PartiallyUpdateOneBook(
        //    [FromRoute(Name = "id")] int id, 
        //    [FromBody] JsonPatchDocument<Book> bookPatch)
        //{
        //    var entity = ApplicationContext
        //        .Books
        //        .Find(x => x.Id.Equals(id));


        //    if (entity is null)
        //        return NotFound(new
        //        {
        //            statusCode = 404,
        //            message = $"Book id{id} not found"
        //        });

        //    bookPatch.ApplyTo(entity);
        //    return NoContent();
        //}
    }
}
