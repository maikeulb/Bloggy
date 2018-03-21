using System;
using System.Linq;
using System.Net;
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
    public class Edit
    {
        public class Command : IRequest<Result>
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public List<Comment> Comments { get; set; } 
            public List<Tag> Tags { get; set; } 
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public CommandHandler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                post.Body = message.Body ?? post.Body;
                post.Title = message.Title ?? post.Body;

                if (message.Tags != null)
                {
                    var oldTags = post.PostTags;
                    post.PostTags.Remove(oldTags);

                    var tags = new List<Tag>();
                    var postTags = new List<PostTag>();
                    foreach(var tag in message.Tags)
                    {
                        var t = await _context.Tags.FindAsync(tag);
                        if (t == null)
                        {
                            t = new Tag() { Name = tag };
                            await _context.Tag.AddAsync(t);
                            await _context.SaveAsync();
                        }
                        tags.Add(t);
                        var pt = new PostTag()
                        {
                            Post = post,
                            Tag = t
                        };
                        postTags.Add(pt);
                    }
                    await _context.PostTag.AddRangeAsync(postTags);
                }

                await _context.Post.AddAsync(post);
                await _context.SaveChangesAsync();

                return Result.Ok ();
            }

            private async Task<Post> SingleAsync(int id)
            {
                return await _context.Post
                    .SingleOrDefault(p => p.Id == id);
            }
        }
    }
}
