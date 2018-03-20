using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using CSharpFunctionalExtensions;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Comments
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            public async Task Handle(Command message, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

                if (post == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                var comment = post.Comments.FirstOrDefault(x => x.CommentId == message.Id);
                if (comment == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
