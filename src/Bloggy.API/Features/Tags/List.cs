using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
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
                public List<Tag> Tags { get; set; } = new List<Tag> ();
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
                return tags.Select(x => x.TagId).ToList()
            }
        }
    }
}
