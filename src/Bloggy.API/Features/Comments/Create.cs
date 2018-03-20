using System;
using AutoMapper;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Comments
{
    public class Create
    {
        public class Command : IRequest<Result<Model>>
        {
            public string Body { get; set; }
        }

        public class Model : IRequest<Result>
        {
            public string Body { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(c => c.Body).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result<Model>>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, 
                    ICurrentUserAccessor currentUserAccessor,
                    IMapper mapper)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            protected override async Task<Result<Model>> HandleCore(Command message)
            {
                var post = await SingleAsync (message.Id);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                var currentUsername = _currentUserAccessor.GetCurrentUsername();
                var author = await _context.Users.FirstAsync(x => x.Username == username);
                
                var comment = new Comment()
                {
                    Author = author,
                    Body = message.Body,
                    CreationDate = DateTime.UtcNow
                };

                post.Comments.Add(comment);

                await _context.Comments.AddAsync(comment);
                await _context.SaveChangesAsync();

                var model = _mapper.Map<Entities.Comment, Model>(comment);

                return Result.Ok (model); 
            }

            private async Task<User> SingleAsync(int id)
            {
                return await _context.Posts
                    .Include(p => p.Comments)
                    .Where(p => p.Id == id)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
