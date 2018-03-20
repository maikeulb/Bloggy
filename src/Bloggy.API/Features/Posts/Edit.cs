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
        public class Query : IRequest<Result<Command>>
        {
            public int Id { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public List<Comment> Comments { get; set; } 
            public List<Tag> Tags { get; set; } 
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(m => m.Id).NotNull();
            }
        }

        public class QueryHandler : AsyncRequestHandler<Query, Result<Command>>
        {
            private readonly BloggyContext _context;

            public QueryHandler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result<Command>> HandleCore(Query message)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                var command = Mapper.Map<Command, Entities.Post> (post);

                return Result.Ok (command);
            }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(m => m.Title).NotEmpty();
                RuleFor(m => m.Body).NotEmpty();
            }
        }

        public class CommandHandler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public CommandHandler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task HandleCore(Command message)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                post.Body = message.Body;
                post.Title = message.Title;

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

                return Result.Ok ();
            }
        }

    private async Task<Post> SingleAsync(int id)
    {
        return await _context.Post
            .SingleOrDefaultAsync(c => c.Id == id);
    }

    }
}
