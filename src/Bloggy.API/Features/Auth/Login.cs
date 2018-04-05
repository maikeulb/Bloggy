using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using Bloggy.API.Features.Users;
using Bloggy.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bloggy.API.Features.Auth
{
    public class Login
    {
        public class Command : IRequest<Result<User>>
        {
            public string Email { get; set; }
            public string Password { get; set; }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator ()
            {
                RuleFor (u => u.Email).NotEmpty ();
                RuleFor (u => u.Password).NotEmpty ();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result<User>>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;
            private readonly ILogger _logger;

            public Handler (BloggyContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper,
                ILogger<AuthController> logger)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
                _logger = logger;
            }

            protected override async Task<Result<User>> HandleCore (Command message)
            {
                var user = await SingleAsync (message.Email);
                if (await SingleAsync (message.Email) == null)
                    return Result.Fail<User> ("User does not exist");

                if (!user.HashedPassword.SequenceEqual (_passwordHasher.Hash (message.Password, user.Salt)))
                    return Result.Fail<User> ("User is not authorized");

                var loggedInUser = _mapper.Map<ApplicationUser, User> (user);

                loggedInUser.Token = await _jwtTokenGenerator.CreateToken (user.Username);

                return Result.Ok (loggedInUser);
            }

            private async Task<ApplicationUser> SingleAsync (string email)
            {
                return await _context.Users
                    .Where (au => au.Email == email)
                    .SingleOrDefaultAsync ();
            }
        }
    }
}
