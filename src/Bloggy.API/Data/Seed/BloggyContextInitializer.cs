using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;
using Bloggy.API.Entities;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using Bloggy.API.Data;

namespace Bloggy.API.Data.Seed
{
    public class BloggyContextInitializer
    {
        public static async Task Initialize(
            BloggyContext context,
            IPasswordHasher passwordHasher,
            ILogger<BloggyContextInitializer> logger)
        {

            context.Database.EnsureCreated();
            
            if (!context.Users.Any())
            {
                var salt = Guid.NewGuid().ToByteArray();
                var hashedPassword = passwordHasher.Hash ("P@ssw0rd!", salt);
                context.Users.AddRange(
                    GetPreconfiguredUsers(hashedPassword, salt));

                await context.SaveChangesAsync();
            }

            if (!context.Tags.Any())
            {
                context.Tags.AddRange(
                    GetPreconfiguredTags());

                await context.SaveChangesAsync();
            }

            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    GetPreconfiguredCategories());

                await context.SaveChangesAsync();
            }

            if (!context.Posts.Any())
            {
                context.Posts.AddRange(
                    GetPreconfiguredPosts());

                await context.SaveChangesAsync();
            }

            if (!context.PostTags.Any())
            {
                context.PostTags.AddRange(
                    GetPreconfiguredPostTags());

                await context.SaveChangesAsync();
            }

            if (!context.Comments.Any())
            {
                context.Comments.AddRange(
                    GetPreconfiguredComments());

                await context.SaveChangesAsync();
            }
        }

        static IEnumerable<ApplicationUser> GetPreconfiguredUsers(byte[] hashedPassword, byte[] salt)
        {
            /* var demoSalt = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bdf").ToByteArray(); */
            /* var demo2Salt = new Guid("25320c5e-f58a-4b1f-b63a-8ee07a840bde").ToByteArray(); */
            return new List<ApplicationUser>()
            {
                new ApplicationUser()
                {
                    Id = 1,
                    Username = "Demo", 
                    Email = "demo@example.com", 
                    HashedPassword = hashedPassword,
                    Salt = salt,
                },
                new ApplicationUser()
                {
                    Id = 2,
                    Username = "Demo2", 
                    Email = "demo2@example.com", 
                    HashedPassword = hashedPassword,
                    Salt = salt,
                },
            };
        }

        static IEnumerable<Tag> GetPreconfiguredTags()
        {
            return new List<Tag>()
            {
                new Tag()
                {
                    Id = 1,
                    Name  = "Csharp"
                },
                new Tag()
                {
                    Id = 2,
                    Name  = "Python"
                },
            };
        }

        static IEnumerable<Category> GetPreconfiguredCategories()
        {
            return new List<Category>()
            {
                new Category()
                {
                    Id = 1,
                    Name  = "Software Architecture",
                },
                new Category()
                {
                    Id = 2,
                    Name  = "Functional Programming",
                },
            };
        }

        static IEnumerable<Post> GetPreconfiguredPosts()
        {
            return new List<Post>()
            {
                new Post()
                {
                    Id = 1,
                    CategoryId = 1,
                    AuthorId = 1,
                    Title  = "integer massa",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 2, 11), 
                },
                new Post()
                {
                    Id = 2,
                    CategoryId = 2,
                    AuthorId = 1,
                    Title  = "adipsicing elit",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 1, 21), 
                },
                new Post()
                {
                    Id = 3,
                    CategoryId = 1,
                    AuthorId = 1,
                    Title  = "efficitur",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 3, 21), 
                },
                new Post()
                {
                    Id = 4,
                    CategoryId = 2,
                    AuthorId = 2,
                    Title  = "sit amet",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 2, 19), 
                },
                new Post()
                {
                    Id = 5,
                    CategoryId = 2,
                    AuthorId = 2,
                    Title  = "pulvinar velit",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 1, 12), 
                },
                new Post()
                {
                    Id = 6,
                    CategoryId = 1,
                    AuthorId = 2,
                    Title  = "dolor lorem",
                    Body  = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                    CreatedDate = new DateTime(2018, 1, 30), 
                },
            };
        }

        static IEnumerable<PostTag> GetPreconfiguredPostTags()
        {
            return new List<PostTag>()
            {
                new PostTag()
                {
                    PostId = 1,
                    TagId = 1,
                },
                new PostTag()
                {
                    PostId = 1,
                    TagId = 2,
                },
                new PostTag()
                {
                    PostId = 2,
                    TagId = 1,
                },
                new PostTag()
                {
                    PostId = 3,
                    TagId = 1,
                },
                new PostTag()
                {
                    PostId = 3,
                    TagId = 2,
                },
                new PostTag()
                {
                    PostId = 4,
                    TagId = 1,
                },
            };
        }

        static IEnumerable<Comment> GetPreconfiguredComments()
        {
            return new List<Comment>()
            {
                new Comment()
                {
                    Id = 1,
                    PostId = 1,
                    AuthorId = 1,
                    Body  = "cool",
                    CreatedDate = new DateTime(2017, 9, 21), 
                },
                new Comment()
                {
                    Id = 2,
                    PostId = 2,
                    AuthorId = 1,
                    Body  = "nice",
                    CreatedDate = new DateTime(2017, 4, 21), 
                },
                new Comment()
                {
                    Id = 3,
                    PostId = 3,
                    AuthorId = 1,
                    Body  = "helpful",
                    CreatedDate = new DateTime(2018, 2, 21), 
                },
                new Comment()
                {
                    Id = 4,
                    PostId = 1,
                    AuthorId = 2,
                    Body  = "good read",
                    CreatedDate = new DateTime(2018, 1, 12), 
                },
                new Comment()
                {
                    Id = 5,
                    PostId = 1,
                    AuthorId = 2,
                    Body  = "wonderful",
                    CreatedDate = new DateTime(2018, 2, 28), 
                },
                new Comment()
                {
                    Id = 6,
                    PostId = 4,
                    AuthorId = 2,
                    Body  = "amazing",
                    CreatedDate = new DateTime(2018, 1, 15), 
                },
            };
        }
    }
}
