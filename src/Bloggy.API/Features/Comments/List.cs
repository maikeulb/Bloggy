using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Comments
{
    public class List
    {
        public class Query : IRequest<Result<Model>>
        {
            public int Id { get; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull();
            }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public DateTime CreationDate { get; set; }
            public ApplicationUser Author { get; set; }
            public Post Post { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Command>>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result<Model>> HandleCore(Query message)
            {
                var post = await _context.Posts

                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                var model = Mapper.Map<Model, Entities.Post.Comments> (post);

                return model;
            }

            private async Task<Post> SingleAsync(int id)
            {
                return await _context.Posts
                    .Include(c => c.Comments)
                    .Where(p => p.Id ==id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
