using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Bloggy.API.Data.Seed
{
    public class BloggyContextInitializer
    {
        public static async Task Initialize (
            BloggyContext context,
            IPasswordHasher passwordHasher,
            ILogger<BloggyContextInitializer> logger)
        {
            context.Database.EnsureCreated ();

            if (!context.Users.Any ())
            {
                var salt = Guid.NewGuid ().ToByteArray ();
                var hashedPassword = passwordHasher.Hash ("P@ssw0rd!", salt);
                context.Users.AddRange (
                    GetPreconfiguredUsers (hashedPassword, salt));

                await context.SaveChangesAsync ();
            }

            if (!context.Tags.Any ())
            {
                context.Tags.AddRange (
                    GetPreconfiguredTags ());

                await context.SaveChangesAsync ();
            }

            if (!context.Categories.Any ())
            {
                context.Categories.AddRange (
                    GetPreconfiguredCategories ());

                await context.SaveChangesAsync ();
            }

            if (!context.Posts.Any ())
            {
                context.Posts.AddRange (
                    GetPreconfiguredPosts ());

                await context.SaveChangesAsync ();
            }

            if (!context.PostTags.Any ())
            {
                context.PostTags.AddRange (
                    GetPreconfiguredPostTags ());

                await context.SaveChangesAsync ();
            }

            if (!context.Comments.Any ())
            {
                context.Comments.AddRange (
                    GetPreconfiguredComments ());

                await context.SaveChangesAsync ();
            }
        }

        static IEnumerable<ApplicationUser> GetPreconfiguredUsers (byte[] hashedPassword, byte[] salt)
        {
            return new List<ApplicationUser> ()
            {
                new ApplicationUser ()
                    {
                        Username = "demo",
                            Email = "demo@example.com",
                            HashedPassword = hashedPassword,
                            Salt = salt,
                    },
                    new ApplicationUser ()
                    {
                        Username = "demo2",
                            Email = "demo2@example.com",
                            HashedPassword = hashedPassword,
                            Salt = salt,
                    },
            };
        }

        static IEnumerable<Tag> GetPreconfiguredTags ()
        {
            return new List<Tag> ()
            {
                new Tag ()
                    {
                        Name = "Csharp"
                    },
                    new Tag ()
                    {
                        Name = "Python"
                    },
            };
        }

        static IEnumerable<Category> GetPreconfiguredCategories ()
        {
            return new List<Category> ()
            {
                new Category ()
                    {
                        Name = "Software Architecture",
                    },
                    new Category ()
                    {
                        Name = "Functional Programming",
                    },
            };
        }

        static IEnumerable<Post> GetPreconfiguredPosts ()
        {
            return new List<Post> ()
            {
                new Post ()
                    {
                        CategoryId = 1,
                            AuthorId = 1,
                            Title = "integer massa",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 2, 11),
                    },
                    new Post ()
                    {
                        CategoryId = 2,
                            AuthorId = 1,
                            Title = "adipsicing elit",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 1, 21),
                    },
                    new Post ()
                    {
                        CategoryId = 1,
                            AuthorId = 1,
                            Title = "efficitur",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 3, 21),
                    },
                    new Post ()
                    {
                        CategoryId = 2,
                            AuthorId = 2,
                            Title = "sit amet",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 2, 19),
                    },
                    new Post ()
                    {
                        CategoryId = 2,
                            AuthorId = 2,
                            Title = "pulvinar velit",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 1, 12),
                    },
                    new Post ()
                    {
                        CategoryId = 1,
                            AuthorId = 2,
                            Title = "dolor lorem",
                            Body = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Vivamus vel pulvinar velit.",
                            CreatedDate = new DateTime (2018, 1, 30),
                    },
            };
        }

        static IEnumerable<PostTag> GetPreconfiguredPostTags ()
        {
            return new List<PostTag> ()
            {
                new PostTag ()
                    {
                        PostId = 1,
                            TagId = 1,
                    },
                    new PostTag ()
                    {
                        PostId = 1,
                            TagId = 2,
                    },
                    new PostTag ()
                    {
                        PostId = 2,
                            TagId = 1,
                    },
                    new PostTag ()
                    {
                        PostId = 3,
                            TagId = 1,
                    },
                    new PostTag ()
                    {
                        PostId = 3,
                            TagId = 2,
                    },
                    new PostTag ()
                    {
                        PostId = 4,
                            TagId = 1,
                    },
            };
        }

        static IEnumerable<Comment> GetPreconfiguredComments ()
        {
            return new List<Comment> ()
            {
                new Comment ()
                    {
                        PostId = 1,
                            AuthorId = 1,
                            Body = "cool",
                            CreatedDate = new DateTime (2017, 9, 21),
                    },
                    new Comment ()
                    {
                        PostId = 2,
                            AuthorId = 1,
                            Body = "nice",
                            CreatedDate = new DateTime (2017, 4, 21),
                    },
                    new Comment ()
                    {
                        PostId = 3,
                            AuthorId = 1,
                            Body = "helpful",
                            CreatedDate = new DateTime (2018, 2, 21),
                    },
                    new Comment ()
                    {
                        PostId = 1,
                            AuthorId = 2,
                            Body = "good read",
                            CreatedDate = new DateTime (2018, 1, 12),
                    },
                    new Comment ()
                    {
                        PostId = 1,
                            AuthorId = 2,
                            Body = "wonderful",
                            CreatedDate = new DateTime (2018, 2, 28),
                    },
                    new Comment ()
                    {
                        PostId = 4,
                            AuthorId = 2,
                            Body = "amazing",
                            CreatedDate = new DateTime (2018, 1, 15),
                    },
            };
        }
    }
}
