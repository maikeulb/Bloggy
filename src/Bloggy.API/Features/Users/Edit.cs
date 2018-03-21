using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Users
{
    public class Edit
    {
        public class Command : IRequest<Result>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler (BloggyContext context, IPasswordHasher passwordHasher,
                ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task<Result> HandleCore (Command message)
            {
                var currentUsername = _currentUserAccessor.GetCurrentUsername ();

                var user = await SingleAsync (currentUsername);

                if (user == null)
                    return Result.Fail<Command> ("User does not exist");

                user.Username = message.Username ?? user.Username;
                user.Email = message.Email ?? user.Email;

                if (!string.IsNullOrWhiteSpace (message.Password))
                {
                    var salt = Guid.NewGuid ().ToByteArray ();
                    user.HashedPassword = _passwordHasher.Hash (message.Password, salt);
                    user.Salt = salt;
                }

                await _context.SaveChangesAsync ();

                return Result.Ok ();
            }

            private async Task<ApplicationUser> SingleAsync (string username)
            {
                return await _context.Users
                    .Where (au => au.Username == username)
                    .SingleOrDefaultAsync ();
            }
        }
    }
}