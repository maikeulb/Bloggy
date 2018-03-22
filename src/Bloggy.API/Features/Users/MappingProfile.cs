using AutoMapper;
using Bloggy.API.Entities;

namespace Bloggy.API.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<ApplicationUser, DetailsQ.Model> ();
        }
    }
}