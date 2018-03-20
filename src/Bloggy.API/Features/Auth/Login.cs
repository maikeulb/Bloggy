using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using Bloggy.API.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Bloggy.API.Features.Users
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
            public Validator()
            {
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command, Result>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly IJwtTokenGenerator _jwtTokenGenerator;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, IPasswordHasher passwordHasher, IJwtTokenGenerator jwtTokenGenerator, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _jwtTokenGenerator = jwtTokenGenerator;
                _mapper = mapper;
            }

            protected override async Task<Result> HandleCore(Command message)
            {
                if (await SingleAsync())
                    return Result.Fail<Command> ("User is not authorized");

                if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, user.Salt)))
                    return Result.Fail<Command> ("User is not authorized");
             
                var user = _mapper.Map<Entities.ApplicationUser, Model>(user);
                /* var user = _mapper.Map<source, destination>(user); */
                user.Token = await _jwtTokenGenerator.CreateToken(user.Username);

                return Result.Ok ();
            }

            private async Task<ApplicationUser> SingleAsync (string email)
            {
                return await _context.ApplicationUsers
                    .Where(au => au.Email == email)
                    .SingeOrDefaultAsync();
            }
        }
    }
}
