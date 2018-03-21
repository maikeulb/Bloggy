using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Data;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using CSharpFunctionalExtensions;
using AutoMapper;

namespace Bloggy.API.Features.Posts
{
    public class Create
    {
        public class Command : IRequest<Result<Model>>
        {
            public string Title { get; set; }
            public string Body { get; set; }
            public List<string> Tags { get; set; }
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

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(p => p.Title).NotEmpty();
                RuleFor(p => p.Body).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result<Model>>
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

            protected override async Task<Result<Model>> HandleCore (Command message)
            {
                var post = new Post
                {
                    Title = message.Title,
                    Body = message.Body,
                    Author = await SingleUserAsync (_currentUserAccessor.GetCurrentUsername()),
                    CreatedDate = DateTime.UtcNow
                };

                var tags = new List<Tag>();
                var postTags = new List<PostTag>();
                foreach(var tag in message.Tags)
                {
                    var t = await SingleTagAsync(tag);
                    if (t == null)
                    {
                        t = new Tag { Name = tag };
                        await _context.Tags.AddAsync(t);
                        await _context.SaveChangesAsync();
                    }
                    tags.Add(t);
                    var pt = new PostTag
                    {
                        Post = post,
                        Tag = t
                    };
                    postTags.Add(pt);
                }

                await _context.Posts.AddAsync(post);
                await _context.PostTags.AddRangeAsync(postTags);
                await _context.SaveChangesAsync();

                var model = _mapper.Map<Post, Model>(post);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> SingleUserAsync(string username)
            {
                return await _context.Users
                    .SingleOrDefaultAsync(au => au.Username == username);
            }

            private async Task<Tag> SingleTagAsync(string name)
            {
                return await _context.Tags
                    .SingleOrDefaultAsync(t => t.Name == name);
            }
        }
    }
}
