using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Posts
{
    public class Delete
    {
        public class Command : IRequest
        {
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;

            public QueryHandler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .FirstOrDefaultAsync(x => x.Slug == message.Slug, cancellationToken);

                if (post == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
