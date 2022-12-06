using AutoMapper;
using BookStoreApp.API.Data;
using BookStoreApp.API.Models.Author;
using BookStoreApp.API.Models.Book;


namespace BookStoreApp.API.Configurations;

public class MapperConfig : Profile
{
    public MapperConfig()
    {
        CreateMap<AuthorCreateDTO, Author>().ReverseMap();
        CreateMap<AuthorUpdateDTO, Author>().ReverseMap();
        CreateMap<AuthorReadOnlyDTO, Author>().ReverseMap();

        CreateMap<Book, BookReadOnlyDTO>()
            .ForMember(
                d => d.AuthorName, 
            s => s.MapFrom(map => $"{map.Author.FirstName} {map.Author.LastName}"))
            .ReverseMap();

        CreateMap<BookCreateDTO, Book>().ReverseMap();
        CreateMap<BookUpdateDTO, Book>().ReverseMap();
    }
}
