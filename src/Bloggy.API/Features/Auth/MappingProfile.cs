using AutoMapper;
using Bloggy.API.Entities;
using Bloggy.API.Features.Users;

namespace Bloggy.API.Features.Auth
{
    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<ApplicationUser, Register.Model> ();
            CreateMap<ApplicationUser, User> ();
            /* CreateMap<User, Login.Model>(); */
        }
    }
}
