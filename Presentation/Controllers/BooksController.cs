using Entities.DataTransferObject;
using Entities.Exeptions;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;


namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))]
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
        public async Task<IActionResult> GetAllBooksAsync()
        {
            var books = await _manager.BookService.GetAllBooksAsync(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);
            return Ok(book);
        }

      
        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPost]
        public async Task <IActionResult> CreateOneBookAsync([FromBody] BookDtoForInsertion bookDto)
        {
            var book = await _manager.BookService.CreateOneBookAsync(bookDto);
            return StatusCode(201, book);
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateOneBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDtoForUpdate bookDto)
        {
            await _manager.BookService.UpdateOneBookAsync(id, bookDto, false);
            return NoContent(); //204
        }

        [HttpDelete("{id:int}")]
        public async Task <IActionResult> DeletOneBooksAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteOneBookAsync(id, false);
            return NoContent();

        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateOneBookAsync(
            [FromRoute(Name = "id")] int id,
            [FromBody] JsonPatchDocument<BookDtoForUpdate> bookPatch)
        {
            if(bookPatch is null) 
                return BadRequest();

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

           // var bookDto = _manager.BookService.GetOneBookById(id, true);

            bookPatch.ApplyTo(result.bookDtoForUpdate, ModelState);

            TryValidateModel(result.bookDtoForUpdate);

            if (!ModelState.IsValid)
                return UnprocessableEntity(ModelState);

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDtoForUpdate, result.book);

            return NoContent(); // 204

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
