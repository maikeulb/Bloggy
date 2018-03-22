using AutoMapper;
using Bloggy.API.Entities;

namespace Bloggy.API.Features.Comments
{
    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<Comment, Create.Model> ();
            CreateMap<Comment, DetailsQ.Model> ();
            CreateMap<Comment, ListAll.Model> ();
        }
    }
}