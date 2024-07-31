using AutoMapper;
using Entities.DataTransferObject;
using Entities.Models;

namespace firstWepApi.Utilities.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<BookDtoForUpdate, Book>();
            CreateMap<Book, BookDto>();
        }
    }
}
