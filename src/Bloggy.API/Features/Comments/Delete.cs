using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using Bloggy.API.Entities;
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
        public class Command : IRequest<Result>
        {
            public int PostId { get; set; }
            public int Id { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.PostId).NotNull();
                RuleFor(c => c.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                var post = await SinglePostAsync(message.PostId);

                if (post == null)
                    return Result.Fail<Command> ("Post does not exit");

                var comment = await SingleCommentAsync(message.Id);

                if (comment == null)
                    return Result.Fail<Command> ("Comment does not exit");

                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();

                return Result.Ok ();
            }

            private async Task<Post> SinglePostAsync(int id)
            {
                return await _context.Posts
                    .Include(x => x.Comments)
                    .SingleOrDefaultAsync(p => p.Id == id);
            }

            private async Task<Comment> SingleCommentAsync(int id)
            {
                return await _context.Comments
                    .SingleOrDefaultAsync(p => p.CommentId == id);
            }
        }
    }
}
