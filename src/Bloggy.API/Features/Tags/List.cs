using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Tags
{
    public class List
    {
        public class Query : IRequest<Model>
        {
            public class Model
            {
                public int Id { get; set; }
                public List<PostTag> PostTags { get; set; } = new List<PostTag> ();
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
                var tags = await _context.Tags.OrderBy(x => x.TagId).AsNoTracking().ToListAsync(cancellationToken);
                return new TagsEnvelope()
                {
                    Tags = tags.Select(x => x.TagId).ToList()
                };
            }
        }
    }
}
