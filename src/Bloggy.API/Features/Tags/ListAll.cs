using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Tags
{
    public class ListAllQ
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

            public Handler (BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore (Query message)
            {
                return new Model { Tags = await _context.Tags.OrderBy (t => t.Name).Select (t => t.Name).AsNoTracking ().ToListAsync () };
            }
        }
    }
}
