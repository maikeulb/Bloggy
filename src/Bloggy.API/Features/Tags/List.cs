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
            public List<Tag> Tags { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Name { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Model> HandleCore (Query message)
            {
                var tags = await _context.Tags.OrderBy(x => x.Name).AsNoTracking().ToListAsync();

                var model = Mapper.Map<IEnumerable<Model>, IEnumerable<Entities.Tag>> (tags);

                return Result.Ok (model);
            }
        }
    }
}
