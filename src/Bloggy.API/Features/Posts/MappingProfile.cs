using System.Linq;
using AutoMapper;
using Bloggy.API.Entities;

namespace Bloggy.API.Features.Posts
{
    public class MappingProfile : Profile
    {
        public MappingProfile ()
        {
            CreateMap<Post, Create.Model> ()
                .ForMember (m => m.Category, opt => opt.MapFrom (p => p.Category.Name))
                .ForMember (m => m.Author, opt => opt.MapFrom (p => p.Author.Username))
                .ForMember (m => m.Tags, opt => opt.MapFrom (p => p.PostTags.Select (pt => pt.Tag.Name)));
            CreateMap<Post, Details.Model> ()
                .ForMember (m => m.Category, opt => opt.MapFrom (p => p.Category.Name))
                .ForMember (m => m.Author, opt => opt.MapFrom (p => p.Author.Username))
                .ForMember (m => m.Tags, opt => opt.MapFrom (p => p.PostTags.Select (pt => pt.Tag.Name)));
            CreateMap<Post, ListAll.Model> ()
                .ForMember (m => m.Category, opt => opt.MapFrom (p => p.Category.Name))
                .ForMember (m => m.Author, opt => opt.MapFrom (p => p.Author.Username))
                .ForMember (m => m.Tags, opt => opt.MapFrom (p => p.PostTags.Select (pt => pt.Tag.Name)));
        }
    }
}
