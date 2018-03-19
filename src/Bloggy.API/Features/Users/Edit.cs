using System;
using System.Linq;
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
    public class Edit
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
                RuleFor(x => x.User).NotNull();
            }
        }

        public class Handler : AsyncRequestHandler<Command>
        {
            private readonly BloggyContext _context;
            private readonly IPasswordHasher _passwordHasher;
            private readonly ICurrentUserAccessor _currentUserAccessor;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, IPasswordHasher passwordHasher, 
                ICurrentUserAccessor currentUserAccessor, IMapper mapper)
            {
                _context = context;
                _passwordHasher = passwordHasher;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            protected override async Task HandleCore(Command message, CancellationToken cancellationToken)
            {
                var currentUsername = _currentUserAccessor.GetCurrentUsername();
                var user = await _context.Users.Where(x => x.Username == currentUsername).FirstOrDefaultAsync(cancellationToken);

                user.Username = message.User.Username ?? user.Username;
                user.Email = message.User.Email ?? user.Email;
                user.Bio = message.User.Bio ?? user.Bio;
                user.Image = message.User.Image ?? user.Image;

                if (!string.IsNullOrWhiteSpace(message.User.Password))
                {
                    var salt = Guid.NewGuid().ToByteArray();
                    user.Hash = _passwordHasher.Hash(message.User.Password, salt);
                    user.Salt = salt;
                }
                
                await _context.SaveChangesAsync(cancellationToken);

                return new UserEnvelope(_mapper.Map<Domain.User, User>(user));
            }
        }
    }
}
