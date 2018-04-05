using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using Bloggy.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bloggy.API.Features.Auth
{
    public class Register
    {
        public class Command : IRequest<Result<Model>>
        {
            public string Username { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Model
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator ()
            {
                RuleFor (u => u.Username).NotEmpty ().WithMessage ("Username cannot be blank");
                RuleFor (u => u.Email).NotEmpty ().WithMessage ("Email cannot be blank")
                    .EmailAddress ().WithMessage ("A valid email is required");
                RuleFor (u => u.Password).NotEmpty ().Length (8, 20).WithMessage ("Password cannot must be between 8 and 20 characters");
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result<Model>>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IMapper _mapper;
            private readonly ILogger _logger;

            public Handler (BloggyContext context, IPasswordHasher passwordHasher, IMapper mapper,
                ILogger<AuthController> logger)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _mapper = mapper;
                _logger = logger;
            }

            protected override async Task<Result<Model>> HandleCore (Command message)
            {
                if (await ExistUser (message.Username))
                    return Result.Fail<Model> ("User exits");

                if (await ExistEmail (message.Email))
                    return Result.Fail<Model> ("Email exits");

                var salt = Guid.NewGuid ().ToByteArray ();
                var user = new ApplicationUser
                {
                    Username = message.Username,
                    Email = message.Email,
                    HashedPassword = _passwordHasher.Hash (message.Password, salt),
                    Salt = salt
                };

                _context.Users.Add (user);
                await _context.SaveChangesAsync ();

                var model = _mapper.Map<ApplicationUser, Model> (user);

                return Result.Ok (model);
            }

            private async Task<Boolean> ExistUser (string username)
            {
                return await _context.Users
                    .Where (au => au.Username == username)
                    .AnyAsync ();
            }

            private async Task<Boolean> ExistEmail (string email)
            {
                return await _context.Users
                    .Where (au => au.Email == email)
                    .AnyAsync ();
            }
        }
    }
}
