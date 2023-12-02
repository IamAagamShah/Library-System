using AutoMapper;
using BookHubAPI.Models;

namespace BookHubAPI.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {

            CreateMap<Review, ReviewDTO>();
            CreateMap<ReviewDTO, Review>();
            CreateMap<Book, BookDTO>();
            CreateMap<BookDTO, Book>();
            CreateMap<UpdateReviewDTO, Review>();
            CreateMap<Review, UpdateReviewDTO>();
            CreateMap<Review, Book>();
            CreateMap<Book, Review>();

        }
    }
}
