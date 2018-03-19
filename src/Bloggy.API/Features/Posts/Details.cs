using System.Net;
using AutoMapper;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using Bloggy.API.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Posts
{
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public ApplicationUser Author { get; set; } 
            public DateTime CreationDate { get; set; }
            public List<Comment> Comments { get; set; } 
            public List<Tag> Tags { get; set; } 
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly ConduitContext _context;

            public QueryHandler(ConduitContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore (Query message, CancellationToken cancellationToken)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                /* var model = Mapper.Map<Model> (post); */
                /* var model = new Model(_mapper.Map<Entities.Post, Model>(post)); */

                var model = Mapper.Map<Model, Entities.Post> (post);

                return Result.Ok (model);
            }

            private async Task<Post> SingleAsync(int id)
            {
                return await _context.Posts
                    .Include(c => c.Author)
                    .Include(c => c.Comments)
                    .Include(c => c.PostTags)
                        .ThenInclude(c => c.Tag)
                    .Where(p => p.Id ==id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
