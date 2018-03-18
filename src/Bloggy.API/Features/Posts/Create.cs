using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Domain;
using Conduit.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Conduit.Features.Posts
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public List<string> Tags { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Title).NotNull();
                RuleFor(c => c.Body).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly ConduitContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext context, 
                    ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                var author = await _context.Persons.FirstAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);
                var tags = new List<Tag>();
                foreach(var tag in (message.Post.TagList ?? Enumerable.Empty<string>()))
                {
                    var t = await _context.Tags.FindAsync(tag);
                    if (t == null)
                    {
                        t = new Tag()
                        {
                            TagId = tag
                        };
                        await _context.Tags.AddAsync(t, cancellationToken);
                        await _context.SaveChangesAsync(cancellationToken);
                    }
                    tags.Add(t);
                }

                var post = new Post()
                {
                    Author = author,
                    Body = message.Post.Body,
                    CreationDate = DateTime.UtcNow,
                    Title = message.Post.Title,
                };
                await _context.Post.AddAsync(post, cancellationToken);
                await _context.PostTags.AddRangeAsync(tags.Select(x => new PostTag()
                {
                    Post = Post,
                    Tag = x
                }), cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
