using AutoMapper;
using Bloggy.API.Entities;

namespace Bloggy.API.Features.Comments
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Comment, Create.Model>();
            CreateMap<Comment, DetailsQ.Model>();
            CreateMap<Comment, ListAll.Model>();
        }
    }
}

            /* // StudentsController */
            /* CreateMap<Student, StudentDetailsViewModel>() */
            /*    .ForMember(vm => vm.Enrollments, */
            /*         opt => opt.MapFrom(s => s.Enrollments.Select(e => new EnrollmentViewModel */
            /*         { */
            /*             Title = e.Course.Title, */
            /*             Grade = ((Grade)e.Grade).ToString() */
            /*         }))); */
            /* CreateMap<Student, StudentViewModel>(); */

            /* // CoursesController */
            /* CreateMap<Course, CourseViewModel>() */
            /*     .ForMember(vm => vm.Department, opt => opt.MapFrom(c => c.Department.Name)) */
            /*     .ForMember(vm => vm.DepartmentID, opt => opt.MapFrom(c => c.DepartmentID)) */
            /*     .ForMember(vm => vm.Assigned, opt => opt.UseValue(false)); */

            /* // InstructorsController */
            /* CreateMap<Instructor, InstructorViewModel>() */
            /*     .ForMember(vm => vm.Office, opt => opt.MapFrom(i => i.OfficeAssignment.Location)) */
            /*     .ForMember(vm => vm.Courses, opt => opt.MapFrom(i => i.CourseAssignments.Select(ca => new CourseViewModel */
            /*     { */
            /*         CourseID = ca.CourseID, */
            /*         Title = ca.Course.Title */
            /*     }))); */
            /* CreateMap<Instructor, InstructorEditViewModel>() */
            /*     .ForMember(vm => vm.Office, opt => opt.MapFrom(i => i.OfficeAssignment.Location)); */
            /* CreateMap<Instructor, InstructorDetailsViewModel>() */
            /*    .ForMember(vm => vm.Courses, */
            /*         opt => opt.MapFrom(i => i.CourseAssignments.Select(ca => new CourseViewModel */
            /*         { */
            /*             CourseID = ca.Course.CourseID, */
            /*             Title = ca.Course.Title, */
            /*             Department = ca.Course.Department.Name */
            /*         }))); */
            /* CreateMap<Course, InstructorDetailsViewModel>() */
            /*    .ForMember(vm => vm.Enrollments, */
            /*         opt => opt.MapFrom(c => c.Enrollments.Select(e => new EnrollmentViewModel */
            /*         { */
            /*             FullName = e.Student.FullName, */
            /*             Grade = ((Grade)e.Grade).ToString() */
            /*         }))); */
