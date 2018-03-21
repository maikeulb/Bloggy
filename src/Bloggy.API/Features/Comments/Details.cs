using System.Net;
using AutoMapper;
using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Tags
{
    public class Details
    {
        public class Query : IRequest<Result<Model>>
        {
            public string PostId { get; set; }
            public string Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public ApplicationUser Author { get; set; }
            public DateTime CreationDate { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(c => c.PostId).NotNull();
                RuleFor(c => c.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Model>>
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
                var comment = await SingleAsync(message.Id);

                if (comment == null)
                    return Result.Fail<Model> ("Comment does not exit");

                var model = _mapper.Map<Entities.Comment, Model>(comment);

                return Result.Ok (model);
            }

            private async Task<Comment> SingleAsync(int id)
            {
                return await _context.Comments
                    .Include(c => c.ApplicationUser)
                    .AsNoTracking()
                    .SingleOrDefaultAsync(p => p.Id == id);
            }
        }
    }
}
