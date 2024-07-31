using Entities.Models;
using Services.Contracts;
using Repositories.Contracts;
using Entities.Exeptions;
using AutoMapper;
using Entities.DataTransferObject;

namespace Services
{
    public class BookManager : IBookService
    {
        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;
        private readonly IMapper _mapper;
        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
        }

        public Book CreateOneBook(Book book)
        {
            _manager.Book.CreateOneBook(book);
            _manager.Save();
            return book;
        }

        public void DeleteOneBook(int id, bool trackChanges)
        {
            var entitiy = _manager.Book.GetOneBookById(id, trackChanges);
            if (entitiy is null)
            {
                throw new BookNotFoundException(id);
            }
                
            _manager.Book.DeleteOneBook(entitiy);
            _manager.Save();
        }

        public IEnumerable<BookDto> GetAllBooks(bool trackChanges)
        {
            var books = _manager.Book.GetAllBooks(trackChanges);
            return _mapper.Map< IEnumerable<BookDto>>(books);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.Book.GetOneBookById(id, trackChanges);
            if (book is null)
                throw new BookNotFoundException(id);
            return book;
        }

        public void UpdateOneBook(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entitiy = _manager.Book.GetOneBookById(id, trackChanges);
            if (entitiy is null)
                throw new BookNotFoundException(id);

            //if(book is null)
            //{
            //    string msg = $"The book whit id:{id} could not found";
            //    _logger.LogInfo(msg);
            //    throw new Exception(msg);
            //}

            //entitiy.Title = book.Title;
            //entitiy.Price = book.Price;
            entitiy = _mapper.Map<Book>(bookDto);
            
            _manager.Book.Update(entitiy);
            _manager.Save();
        }
    }
}
