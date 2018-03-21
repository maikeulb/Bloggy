using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using Bloggy.API.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Posts
{
    public class List
    {
        public class Query : IRequest<Model>
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

            public List<PostTag> PostTags { get; set; }
            public List<Comment> Comments { get; set; }
            public List<Tag> Tags { get; set; } 
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(BloggyContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task<Model> HandleCore (Query message)
            {
                IQueryable<Post> queryablePosts = ListPosts();

                if (!string.IsNullOrWhiteSpace(message.Tag))
                {
                    var tag = await SingleTagAsync(message.Tag);
                    if (tag != null)
                        queryablePosts = queryablePosts.Where(p => p.PostTags.Select(pt => pt.Tag.Name).Contains(tag.Name));
                    else
                        return new Post();
                }
                if (!string.IsNullOrWhiteSpace(message.Author))
                {
                    var author = await SingleAuthorAsync(message.Author.username);
                    if (author != null)
                        queryablePosts = queryablePosts.Where(p => p.Author == message.Author);
                    else
                        return new Model();
                }

                var posts = await queryablePosts
                    .OrderByDescending(m => m.CreationDate)
                    .AsNoTracking()
                    .ToListAsync();

                var model = Mapper.Map<Model, Entities.Post> (posts);

                return model;
            }

            private async Task<Post> SingleTagAsync(string tag)
            {
                return await _context.PostTags
                    .Include(pt => pt.Tags)
                    .SingleOrDefaultAsync(pt => pt.Tags.Name == tag);
            }

            private async Task<ApplicationUser> SingleAuthorAsync(string author)
            {
                return await _context.ApplicationUsers
                    .SingleOrDefaultAsync(au => au.Username == author);
            }

            public IQueryable<Post> ListPosts()
            {
                return _context.Posts
                    .Include(x => x.Author)
                    .Include(x => x.ArticleFavorites)
                    .Include(x => x.ArticleTags)
                    .AsNoTracking();
            }
        }
    }
}
