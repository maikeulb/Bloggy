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

namespace Bloggy.API.Features.Posts
{
    public class Details
    {
        public class Query : IRequest<Result<Model>>
        {
            public int Id { get; set; }
        }

        public class Model
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Body { get; set; }
            public string Category { get; set; }
            public string Author { get; set; }
            public List<string> Tags { get; set; }
            public List<Comment> Comments { get; set; }
            public DateTime CreatedDate { get; set; }

            public class Comment
            {
                public int Id { get; set; }
                public string Body { get; set; }
                public ApplicationUser Author { get; set; }
                public DateTime CreatedDate { get; set; }

                public class ApplicationUser
                {
                    public int Id { get; set; }
                    public string Username { get; set; }
                }
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator ()
            {
                RuleFor (p => p.Id).NotNull ();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Model>>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;

            public Handler (BloggyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Result<Model>> HandleCore (Query message)
            {
                var post = await SingleAsync (message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                var model = _mapper.Map<Post, Model> (post);

                return Result.Ok (model);
            }

            private async Task<Post> SingleAsync (int id)
            {
                return await _context.Posts
                    .Include (c => c.Author)
                    .Include (c => c.Category)
                    .Include (c => c.PostTags)
                        .ThenInclude (c => c.Tag)
                    .Include (c => c.Comments)
                        .ThenInclude (c => c.Author)
                    .AsNoTracking ()
                    .SingleOrDefaultAsync (p => p.Id == id);
            }
        }
    }
}
