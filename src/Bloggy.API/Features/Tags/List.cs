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
            public List<string> Tags { get; set; }
        }

        public class Model
        {
            public List<string> Tags { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context, IMapper _mapper)
            {
                _context = context;
            }

            protected override async Task<IEnumerable<Model>> HandleCore (Query message)
            {
                return new Model() { Tags = await _context.Tags.OrderBy(x => x.Name).AsNoTracking().ToListAsync();}
            }
        }
    }
}
