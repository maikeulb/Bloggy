using System;
using System.Linq;
using System.Net;
using CSharpFunctionalExtensions;
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
    public class Edit
    {
        public class Query : IRequest<Result<Command>>
        {
            public int Id { get; set; }
            public int PostId { get; set; }
        }

        public class Command : IRequest<Result>
        {
            public string Body { get; set; }
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

                var currentUsername = await _context.Users.FirstAsync(x => x.Username == username);

                if (post.Author.username != currentUsername)
                    return Result.Fail<Command> ("User is not authorized");

                var comment = new Comment()
                {
                    Body = message.Body,
                };

                return Result.Ok (command);
            }

        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
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

            protected override async Task<Result> HandleCore(Command message, CancellationToken cancellationToken)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                post.Body = message.Body;

                await _context.SaveChangesAsync();
            }

            private async Task<User> SingleAsync(int id)
            {
                return await _context.Posts
                    .Include(p => p.Comments)
                    .Where(p => p.Id == id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
