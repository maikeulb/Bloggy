using System.Net;
using CSharpFunctionalExtensions;
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
        public class Command : IRequest<Result>
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

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public QueryHandler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                var post = await SinglePostAsync(message.Id)

                if (post == null)
                    return Result.Fail<Command> ("Post does not exit");

                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }

            private async Task<Post> SinglePostAsync(int id)
            {
                return await _context.Posts
                    .Where(p => p.Id == id)
                    .SingleOrDefaultAsync();
            }

        }
    }
}
