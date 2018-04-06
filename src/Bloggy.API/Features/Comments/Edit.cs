using System;
using System.Threading.Tasks;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using Bloggy.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Comments
{
    public class Edit
    {
        public class Command : IRequest<Result>
        {
            public int PostId { get; set; }
            public int Id { get; set; }
            public string Body { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator ()
            {
                RuleFor (c => c.PostId).NotNull ();
                RuleFor (c => c.Id).NotNull ();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler (BloggyContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task<Result> HandleCore (Command message)
            {
                var post = await SinglePostAsync (message.PostId);

                if (post == null)
                    return Result.Fail<Command> ("Post does not exit");

                if (message.Body != null)
                {
                    var existComment = await SingleCommentAsync (message.Id);
                    _context.Comments.Remove (existComment);
                    await _context.SaveChangesAsync ();

                    var comment = new Comment
                    {
                        Author = await SingleUserAsync (_currentUserAccessor.GetCurrentUsername ()),
                        Body = message.Body,
                        CreatedDate = DateTime.UtcNow
                    };

                    await _context.Comments.AddAsync (comment);
                    post.Comments.Add (comment);
                    await _context.SaveChangesAsync ();
                }

                return Result.Ok ();
            }

            private async Task<Post> SinglePostAsync (int id)
            {
                return await _context.Posts
                    .Include (p => p.Comments)
                    .SingleOrDefaultAsync (p => p.Id == id);
            }

            private async Task<Comment> SingleCommentAsync (int id)
            {
                return await _context.Comments
                    .SingleOrDefaultAsync (c => c.Id == id);
            }

            private async Task<ApplicationUser> SingleUserAsync (string username)
            {
                return await _context.Users
                    .SingleOrDefaultAsync (u => u.Username == username);
            }
        }
    }
}
