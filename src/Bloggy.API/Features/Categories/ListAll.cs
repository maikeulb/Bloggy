using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Bloggy.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Categories
{
    public class ListAllQ
    {
        public class Query : IRequest<Model>
        {
            public List<string> Categories { get; set; }
        }

        public class Model
        {
            public List<string> Categories { get; set; }
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
                return new Model { Categories = await _context.Categories.OrderBy (c => c.Name).Select (c => c.Name).AsNoTracking ().ToListAsync () };
            }
        }
    }
}
