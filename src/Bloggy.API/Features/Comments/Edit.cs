using System;
using System.Linq;
using System.Net;
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
            public Validator()
            {
                RuleFor(c => c.PostId).NotNull();
                RuleFor(c => c.Id).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;

            public Handler(BloggyContext context)
            {
                _context = context;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                var post = await SinglePostAsync(message.PostId);

                if (post == null)
                    return Result.Fail<Model> ("Post does not exit");

                if (message.Body != null)
                {
                    var comment = await SingleCommentAsync(message.Id);
                    _context.Comments.Remove(comment);
                    await _context.SaveChangesAsync();

                    var comment = new Comment
                    {
                        Author = await SingleAsync (_currentUserAccessor.GetCurrentUsername()),
                        Body = message.Body,
                        CreationDate = DateTime.UtcNow
                    }

                    await _context.Comments.AddAsync(comment);
                    post.Comments.Add(comment);
                    await _context.SaveChangesAsync();
                } 

                return Result.Ok ()

            private async Task<Post> SinglePostAsync(int id)
            {
                return await _context.Posts
                    .Include(p => p.Comments)
                    .SingleOrDefaultAsync(p => p.Id == id);
            }

            private async Task<Comment> SingleCommentAsync(int id)
            {
                return await _context.Comments
                    .SingleOrDefaultAsync(c => c.Id == id);
            }

            private async Task<ApplicationUser> SingleUserAsync(string username)
            {
                return await _context.ApplicationUsers
                    .SingleOrDefaultAsync(au => au.Username == username);
        }
    }
}

 // ananymous types
 // var sumeProp = _context.Samurais.Select(s => new {s.Id,
 // s.Name}).ToList();
 //
 //_context.Quotes.Text +="hi"
 // delete
 //_context.Quotes.Remove(samurai.Quotrs)
 //
 //Find does not need asnotracking
 //asnotracking k
 //
 //_context.Add(samura)
 //_context.Update(samurai)
 //_context.Samurais.Remove(samurai);
 //use find for list<T>
 //
 //use where/firstordefault for ienumerable<T>
 // IQueryable<Order> _context.Orders
 // Order Find(int id) _context.Orders.Find(id)
 // Insert _context.Orders.Add(order);
 // _context.Orders.Update(order);
 // _context.Orders.Remove(order);
 //
 //AsNoTracking
