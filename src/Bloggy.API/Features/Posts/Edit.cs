using System.Collections.Generic;
using System.Threading.Tasks;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using CSharpFunctionalExtensions;
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
            public int CategoryId { get; set; }
            public List<string> Tags { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator ()
            {
                RuleFor (p => p.Id).NotNull ();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public Handler (BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore (Command message)
            {
                var post = await SingleAsync (message.Id);
                if (post == null)
                    return Result.Fail<Command> ("Post does not exit");

                var category = await SingleCategoryAsync (message.CategoryId);
                if (category == null)
                    return Result.Fail<Command> ("Category does not exit");

                post.Title = message.Title ?? post.Title;
                post.Body = message.Body ?? post.Body;
                post.CategoryId = message.CategoryId;

                if (message.Tags != null)
                {
                    post.PostTags.Clear ();

                    var tags = new List<Tag> ();
                    var postTags = new List<PostTag> ();
                    foreach (var tag in message.Tags)
                    {
                        var t = await SingleTagAsync (tag);
                        if (t == null)
                        {
                            t = new Tag { Name = tag };
                            await _context.Tags.AddAsync (t);
                            await _context.SaveChangesAsync ();
                        }
                        tags.Add (t);
                        var pt = new PostTag
                        {
                            Post = post,
                            Tag = t
                        };
                        postTags.Add (pt);
                    }
                    await _context.PostTags.AddRangeAsync (postTags);
                }

                await _context.SaveChangesAsync ();

                return Result.Ok ();
            }

            private async Task<Post> SingleAsync (int id)
            {
                return await _context.Posts
                    .Include (c => c.Category)
                    .Include (c => c.PostTags)
                    .ThenInclude (c => c.Tag)
                    .SingleOrDefaultAsync (p => p.Id == id);
            }

            private async Task<Tag> SingleTagAsync (string name)
            {
                return await _context.Tags
                    .SingleOrDefaultAsync (t => t.Name == name);
            }

            private async Task<Category> SingleCategoryAsync (int id)
            {
                return await _context.Categories
                    .SingleOrDefaultAsync (c => c.Id == id);
            }
        }
    }
}
