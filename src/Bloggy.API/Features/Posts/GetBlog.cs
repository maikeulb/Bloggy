using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Posts
{
    public class GetBlog
    {
        public class Query : IRequest<Model>
        {
            public Query
            {
                public string Tag { get; }
                public string Author { get; }
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
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(ConduitContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task<Model> HandleCore (Query message, CancellationToken cancellationToken)
            {
                IQueryable<Post> queryable = _context.Posts.GetAllData();

                if (!string.IsNullOrWhiteSpace(message.Tag))
                {
                    var tag = await _context.PostTags.FirstOrDefaultAsync(x => x.TagId == message.Tag, cancellationToken);
                    if (tag != null)
                    {
                        queryable = queryable.Where(x => x.PostTags.Select(y => y.TagId).Contains(tag.TagId));
                    }
                    else
                    {
                        return new Post();
                    }
                }
                if (!string.IsNullOrWhiteSpace(message.Author))
                {
                    var author = await _context.Persons.FirstOrDefaultAsync(x => x.Username == message.Author, cancellationToken);
                    if (author != null)
                    {
                        queryable = queryable.Where(x => x.Author == author);
                    }
                    else
                    {
                        return new Model();
                    }
                }

                var model = await queryable
                    .OrderByDescending(m => m.CreationDate)
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
