using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Posts
{
    public class Create
    {
        public class Command : IRequest<Model>
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public List<string> Tags { get; set; } = new List<string> Tags();
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public ApplicationUser Author { get; set; } 
            public DateTime CreationDate { get; set; }
            public List<Comment> Comments { get; set; } 
            public List<Tag> Tags { get; set; } 
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Title).NotEmpty();
                RuleFor(c => c.Body).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(BloggyContext context,ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task HandleCore(Command message)
            {
                var author = await SingleAsync (_currentUserAccessor.GetCurrentUsername());

                if (author == null)
                    return Result.Fail<Model> ("Author does not exit");

                var post = new Post()
                {
                    Title = message.Title,
                    Body = message.Body,
                    Author = author,
                    CreationDate = DateTime.UtcNow,
                };

                var tags = new List<Tag>();
                var postTags = new List<PostTag>();
                foreach(var tag in message.Tags)
                {
                    var t = await _context.Tags.FindAsync(tag);
                    if (t == null)
                    {
                        t = new Tag() 
                        {
                            Name = tag
                        };
                        await _context.Tag.AddAsync(t);
                        await _context.SaveAsync()
                    }
                    tags.Add(t);
                    var pt = new PostTag()
                    {
                        Post = post,
                        Tag = t
                    };
                    postTags.Add(pt);
                }

                await _context.Post.AddAsync(post);
                await _context.PostTag.AddRangeAsync(postTags);
                await _context.SaveChangesAsync()

                var model = Mapper.Map<Model, Entities.Post> (post);

                return Result.Ok (model);
            }

            private async Task<User> SingleAsync(string username)
            {
                return await _context.Users
                    .Where(u => u.Username == username)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
