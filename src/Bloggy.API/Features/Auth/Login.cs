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
                RuleFor(x => x.Email).NotNull().NotEmpty();
                RuleFor(x => x.Password).NotNull().NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
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

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                var user = await _context.ApplicationUsers.Where(x => x.Email == message.User.Email).SingleOrDefaultAsync(cancellationToken);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }

                if (!user.Hash.SequenceEqual(_passwordHasher.Hash(message.User.Password, user.Salt)))
                {
                    throw new RestException(HttpStatusCode.Unauthorized);
                }
             
                var user  = _mapper.Map<Domain.User, User>(user); ;
                user.Token = await _jwtTokenGenerator.CreateToken(user.Username);
            }
        }
    }
}
