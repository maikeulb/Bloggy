using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using Bloggy.API.Data;
using MediatR;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System;
using AutoMapper;

namespace Bloggy.API.Features.Posts
{
    public class ListAll
    {
        public class Query : IRequest<IEnumerable<Model>>
        {
            public string Tag { get; }
            public string Author { get; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public DateTime CreatedDate { get; set; }
            public ApplicationUser Author { get; set; }
            public List<Comment> Comments { get; set; }
            public List<Tag> Tags { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            protected override async Task<IEnumerable<Model>> HandleCore (Query message)
            {
                IQueryable<Post> queryablePosts = ListPosts();

                if (!string.IsNullOrWhiteSpace(message.Tag))
                {
                    var tag = await SingleTagAsync(message.Tag);
                    if (tag != null)
                        queryablePosts = queryablePosts.Where(p => p.PostTags.Select(pt => pt.Tag.Name).Contains(tag.Name));
                }
                if (!string.IsNullOrWhiteSpace(message.Author))
                {
                    var author = await SingleAuthorAsync(message.Author);
                    if (author != null)
                        queryablePosts = queryablePosts.Where(p => p.Author.Username == message.Author);
                }

                var posts = await queryablePosts
                    .OrderByDescending(m => m.CreatedDate)
                    .AsNoTracking()
                    .ToListAsync();

                var model = _mapper.Map<IEnumerable<Post>, IEnumerable<Model>>(posts);

                return model;
            }

            private async Task<Tag> SingleTagAsync(string tag)
            {
                return await _context.Tags
                    .SingleOrDefaultAsync(pt => pt.Name == tag);
            }

            private async Task<ApplicationUser> SingleAuthorAsync(string author)
            {
                return await _context.Users
                    .SingleOrDefaultAsync(au => au.Username == author);
            }

            public IQueryable<Post> ListPosts()
            {
                return _context.Posts
                    .Include(x => x.Author)
                    .Include(x => x.PostTags)
                    .AsNoTracking();
            }
        }
    }
}
