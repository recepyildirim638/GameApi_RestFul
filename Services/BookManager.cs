﻿using Entities.Models;
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

        public async Task<BookDto> CreateOneBookAsync(BookDtoForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);
            _manager.Book.CreateOneBook(entity);
            await _manager.SaveAsync();
            return _mapper.Map<BookDto>(entity);
        }

        public async Task DeleteOneBookAsync(int id, bool trackChanges)
        {
            var entitiy = await GetOneBookByIdAndCheckExits(id, trackChanges);
            _manager.Book.DeleteOneBook(entitiy);
            await _manager.SaveAsync();
        }

        public async Task<IEnumerable<BookDto>> GetAllBooksAsync(bool trackChanges)
        {
            var books = await _manager.Book.GetAllBooksAsync(trackChanges);
            return _mapper.Map< IEnumerable<BookDto>>(books);
        }

        public async Task<BookDto> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExits(id, trackChanges);
            //if (book is null)
            //    throw new BookNotFoundException(id);
            return _mapper.Map<BookDto>(book);
        }

        public async Task<(BookDtoForUpdate bookDtoForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExits(id, trackChanges);
            var bookDtoForUpdate = _mapper.Map<BookDtoForUpdate>(book);
            return (bookDtoForUpdate, book);
        }

        public async Task SaveChangesForPatchAsync(BookDtoForUpdate bookDtoForUpdate, Book book)
        {
            _mapper.Map(bookDtoForUpdate, book);
            await _manager.SaveAsync();
        }

        public async Task UpdateOneBookAsync(int id, BookDtoForUpdate bookDto, bool trackChanges)
        {
            var entitiy = await GetOneBookByIdAndCheckExits(id, trackChanges);
            entitiy = _mapper.Map<Book>(bookDto);
            
            _manager.Book.Update(entitiy);
            await _manager.SaveAsync();
        }

        private async Task<Book> GetOneBookByIdAndCheckExits(int id, bool trackChanges)
        {
            var entitiy = await _manager.Book.GetOneBookByIdAsync(id, trackChanges);

            if (entitiy is null)
                throw new BookNotFoundException(id);

            return entitiy;
        }
    }
}
