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
            public int PostId { get; set; }
            public int Id { get; set; }
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
            private readonly ApplicationDbContext _context;
            private readonly IUrlComposer _urlComposer;

            public QueryHandler(ApplicationDbContext context,
                IUrlComposer urlComposer)
            {
                _context = context;
                _urlComposer = urlComposer;
            }

            protected override async Task<Result<Command>> HandleCore(Query message)
            {
                var catalogItem = await SingleAsync(message.Id);

                if (catalogItem == null)
                    return Result.Fail<Command> ("Catalog Item does not exit");

                var command =  new Command
                {
                    Id = catalogItem.Id,
                    Name = catalogItem.Name,
                    Brand = catalogItem.CatalogBrand.Brand,
                    Type = catalogItem.CatalogType.Type,
                    Description = catalogItem.Description,
                    Stock = catalogItem.AvailableStock,
                    Price = catalogItem.Price,
                    ImageUrl = _urlComposer.ComposeImgUrl(catalogItem.ImageUrl)
                };

                return Result.Ok (command);
            }

            private async Task<CatalogItem> SingleAsync(int id)
            {
                return await _context.CatalogItems
                    .Include(c => c.CatalogBrand)
                    .Include(c => c.CatalogType)
                    .SingleOrDefaultAsync(c => c.Id == id);
            }
        }

        public class Command : IRequest<Result>
        {
            public string Body { get; set; }
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
                var post = await _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == message.PostId, cancellationToken);

                if (post == null)
                    throw new RestException(HttpStatusCode.NotFound);

                post.Body = message.Body;

                if (_context.ChangeTracker.Entries().First(x => x.Entity == post).State == EntityState.Modified)
                    post.UpdatedAt = DateTime.UtcNow;
                
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}
