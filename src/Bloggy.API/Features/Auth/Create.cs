using System;
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
    public class Create
    {
        public class Command : IRequest<Result>
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

        public class Handler : AsyncRequestHandler<Command, Result>
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

            protected override async Task<Result> HandleCore(Command message)
            {
                if (await ExistUser(message.Username))
                    return Result.Fail<Model> ("User does not exit");

                if (await ExistEmail(message.Email))
                    return Result.Fail<Model> ("User does not exit");

                var model = new Model
                {
                    Username = message.Username,
                    Email = message.Email,
                    HashedPassword = _passwordHasher.Hash(message.Password, salt),
                    Salt = Guid.NewGuid().ToByteArray()
                };

                _context.ApplicationUsers.Add(user);
                await _context.SaveChangesAsync();

                var model = Mapper.Map<Model, Entities.ApplicationUser> (user);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> ExistUser(int username)
            {
                return await _context.ApplicationUsers
                    .Where(au => au.Username == username)
                    .AnyAsync();
            }

            private async Task<ApplicationUser> ExistEmail(int email)
            {
                return await _context.ApplicationUsers
                    .Where(au => au.Email == email)
                    .AnyAsync();
            }
        }
    }
}
