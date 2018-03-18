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
        public class Query : IRequest<CommentsEnvelope>
        {
            public Query
            {
                public int Id { get; }
            }

            public class Model
            {
                public int Id { get; set; }
                public string Body { get; set; }
                public DateTime CreationDate { get; set; }

                public ApplicationUser Author { get; set; }
                public Post Post { get; set; }
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore (Query message, CancellationToken cancellationToken)
            {
                var post = await _context.Posts
                    .Include(x => x.Comments)
                    .FirstOrDefaultAsync(x => x.Id == message.Id, cancellationToken);

                if (post == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }

                return post;
            }
        }
    }
}
