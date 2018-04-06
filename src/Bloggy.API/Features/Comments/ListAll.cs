using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Comments
{
    public class ListAll
    {
        public class Query : IRequest<Result<IEnumerable<Model>> >
        {
            public int PostId { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator ()
            {
                RuleFor (c => c.PostId).NotNull ();
            }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Body { get; set; }
            public ApplicationUser Author { get; set; }
            public DateTime CreatedDate { get; set; }
        }

        public class Handler : AsyncRequestHandler<Query, Result<IEnumerable<Model>> >
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;

            public Handler (BloggyContext context,
                IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Result<IEnumerable<Model>> > HandleCore (Query message)
            {
                var post = await SingleAsync (message.PostId);

                if (post == null)
                    return Result.Fail<IEnumerable<Model>> ("Post does not exit");

                var model = _mapper.Map<IEnumerable<Comment>, IEnumerable<Model>> (post.Comments);

                return Result.Ok (model);
            }

            private async Task<Post> SingleAsync (int id)
            {
                return await _context.Posts
                    .Include (c => c.Comments)
                    .AsNoTracking ()
                    .SingleOrDefaultAsync (p => p.Id == id);
            }
        }
    }
}
