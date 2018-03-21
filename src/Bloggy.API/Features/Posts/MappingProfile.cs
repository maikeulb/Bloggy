using System.Linq;
using AutoMapper;
using Bloggy.API.Entities;

namespace Bloggy.API.Features.Posts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Post, Create.Model>()
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.PostTags.Select(pt => pt.Tag.Name)))
                .ForMember(m => m.Category, opt => opt.MapFrom(p => p.Category.Name));
            CreateMap<Post, Details.Model>()
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.PostTags.Select(pt => pt.Tag.Name)))
                .ForMember(m => m.Category, opt => opt.MapFrom(p => p.Category.Name));
            CreateMap<Post, ListAll.Model>()
                .ForMember(m => m.Tags, opt => opt.MapFrom(p => p.PostTags.Select(pt => pt.Tag.Name)))
                .ForMember(m => m.Category, opt => opt.MapFrom(p => p.Category.Name));
        }
    }
}

    /* cfg.CreateMap<UserEntity, UserViewModel>() */
    /*         .ForMember( */
    /*                             dto => dto.Tags, */
    /*                                         opt => opt.MapFrom(x => x.UserAndTags.Select(y => y.Tag))); */

/* Mapper.CreateMap<Student,StudentDTO> */
/* .ForMember(s => s.Images, opt=>opt.MapFrom(p=>p.Images)); */

            /* // InstructorsController */
            /* CreateMap<Instructor, InstructorViewModel>() */
            /*     .ForMember(vm => vm.Office, opt => opt.MapFrom(i => i.OfficeAssignment.Location)) */
            /*     .ForMember(vm => vm.Courses, opt => opt.MapFrom(i => i.CourseAssignments.Select(ca => new CourseViewModel */
            /*     { */
            /*         CourseID = ca.CourseID, */
            /*         Title = ca.Course.Title */
            /*     }))); */

/* Mapper.CreateMap<User, UserDTO>() */
/*     .ForMember(dto => dto.Companies, opt => opt.MapFrom(x => x.UserCompanies)); */

/* Mapper.CreateMap<Company, CompanyDTO>(); */
/* Mapper.CreateMap<User, UserDTO>() */
/*     .ForMember(dto => dto.Companies, opt => opt.ResolveUsing<CompanyResolver>()); */
/* Mapper.AssertConfigurationIsValid(); */

/* public class CompanyResolver : ValueResolver<User, IList<CompanyDTO>> */
/* { */
/*     protected override IList<CompanyDTO> ResolveCore(User source) */
/*     { */
/*         return source.UserCompanies */
/*             .Select(userCompany => */
/*                     Mapper.Map<Company, CompanyDTO>(companies.FirstOrDefault(x => x.CompanyID == userCompany.CompanyID))) */
/*             .ToList(); */
/*     } */
/* } */

/* Mapper.CreateMap<Student, StudentIDO>(); */

/* Mapper.CreateMap<StudentImage, ImageDTO>() */
/*             .ForMember(d => d.Id, opt => opt.MapFrom(s => s.ImageId)) */
/*             .ForMember(d => d.Filename, opt => opt.MapFrom(s => s.Image.Filename)); */

            /* Mapper.CreateMap<StudentIDO, Student>() */
          /* .AfterMap((s, d) => */
            /*             { */
            /*                       foreach (var studentImage in d.Images) */
            /*                                     studentImage.StudentId = s.Id; */
            /*                                           }); */

/* Mapper.CreateMap<ImageDTO, StudentImage>() */
/*           .ForMember(d => d.ImageId, opt => opt.MapFrom(s => s.Id)); */
