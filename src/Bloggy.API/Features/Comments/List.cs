using System.Net;
using AutoMapper;
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
        public class Query : IRequest<Result<Model>>
        {
            public int Id { get; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Id).NotNull();
            }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public ApplicationUser Author { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Command>>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context,
                    IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Result<Model>> HandleCore(Query message)
            {
                var post = await SingleAsync(message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                var model = _mapper.Map<Entities.Post.Comments, Model>(post);

                return model;
            }

            private async Task<Post> SingleAsync(int id)
            {
                return await _context.Posts
                    .Include(c => c.Comments)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id ==id);
            }
        }
    }
}
