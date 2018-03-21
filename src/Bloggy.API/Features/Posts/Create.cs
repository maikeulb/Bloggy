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
            public List<string> Tags { get; set; } 
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
                var post = new Post()
                {
                    Title = message.Title,
                    Body = message.Body,
                    Author = await SingleUserAsync (_currentUserAccessor.GetCurrentUsername()),
                    CreationDate = DateTime.UtcNow,
                };

                var tags = new List<Tag>();
                var postTags = new List<PostTag>();
                foreach(var tag in message.Tags)
                {
                    var t = await _context.Tags.SingleTagAsync(tag);
                    if (t == null)
                    {
                        t = new Tag() { Name = tag };
                        await _context.Tag.AddAsync(t);
                        await _context.SaveChangesAsync();
                    }
                    tags.Add(t);
                    var pt = new PostTag()
                    {
                        Post = post,
                        Tag = t
                    };
                    postTags.Add(pt);
                }

                await _context.Posts.AddAsync(post);
                await _context.PostTags.AddRangeAsync(postTags);
                await _context.SaveChangesAsync()

                var model = _mapper.Map<Entities.Post, Model>(post);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> SingleUserAsync(string username)
            {
                return await _context.Users
                    .SingleOrDefaultAsync(u => u.Username == username);
            }

            private async Task<Tag> SingleTagAsync(string name)
            {
                return await _context.Tags
                    .SingleOrDefaultAsync(t => t.Name == name);
            }
        }
    }
}
        }
    }
}
