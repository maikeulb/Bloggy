using System;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Users
{
    public class Create
    {
        public class Command : IRequest
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, IPasswordHasher passwordHasher, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
            }

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                if (await _context.Users.Where(x => x.Username == message.User.Username).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }

                if (await _context.Users.Where(x => x.Email == message.User.Email).AnyAsync(cancellationToken))
                {
                    throw new RestException(HttpStatusCode.BadRequest);
                }

                var salt = Guid.NewGuid().ToByteArray();
                var model = new Model
                {
                    Username = message.User.Username,
                    Email = message.ApplicationUser.Email,
                    HashedPassword = _passwordHasher.Hash(message.ApplicationUser.Password, salt),
                    Salt = salt
                };

                _context.ApplicationUsers.Add(user);
                await _context.SaveChangesAsync(cancellationToken);

                return user
            }
        }
    }
}
