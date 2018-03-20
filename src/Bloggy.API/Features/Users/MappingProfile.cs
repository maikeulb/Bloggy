using AutoMapper;

namespace Bloggy.API.Features.Users
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Entities.ApplicationUser, Details.Model>();
        }
    }
}
