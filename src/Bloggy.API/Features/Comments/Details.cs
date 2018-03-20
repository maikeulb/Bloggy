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

namespace Bloggy.API.Features.Tags
{
    public class Details
    {
        public class Query : IRequest<ArticleEnvelope>
        {
            public string Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public DateTime CreationDate { get; set; }

            public ApplicationUser Author { get; set; }
            public Post Post { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Slug).NotNull().NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            public async Task<ArticleEnvelope> Handle(Query message, CancellationToken cancellationToken)
            {
                var comment = await SingleAsync(message.Id);

                if (comment == null)
                    return Result.Fail<Model> ("Comment does not exit");

                var model = Mapper.Map<Model, Entities.Comment> (comment);

                return Result.Ok (model);
            }

            private async Task<Post> SingleAsync(int id)
            {
                return await _context.Posts
                    .Where(p => p.Id ==id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
