using System;
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

namespace Bloggy.API.Features.Comments
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Body { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Body).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(BloggyContext context, 
                    ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

                if (post == null)
                    throw new RestException(HttpStatusCode.NotFound);

                var author = await _context.Users.FirstAsync(x => x.Username == _currentUserAccessor.GetCurrentUsername(), cancellationToken);
                
                var comment = new Comment()
                {
                    Author = author,
                    Body = message.Comment.Body,
                    CreationDate = DateTime.UtcNow,
                };
                await _context.Comments.AddAsync(comment, cancellationToken);

                post.Comments.Add(comment);

                await _context.SaveChangesAsync(cancellationToken);

                return comment
            }
        }
    }
}
