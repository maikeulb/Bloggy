using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Auth
{
    public class Login
    {
        public class Command : IRequest<Result>
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

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler (BloggyContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            protected override async Task<Result> HandleCore (Command message)
            {
                var user = await SingleAsync (message.Email);
                if (await SingleAsync (message.Email) != null)
                    return Result.Fail<Command> ("User is not authorized");

                if (!user.HashedPassword.SequenceEqual (_passwordHasher.Hash (message.Password, user.Salt)))
                    return Result.Fail<Command> ("User is not authorized");

                /* var user = _mapper.Map<Entities.ApplicationUser, Model>(user); */
                /* user.Token = await _jwtTokenGenerator.CreateToken(user.Username); */

                return Result.Ok ();
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