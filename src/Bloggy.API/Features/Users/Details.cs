using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Bloggy.API.Features.Users
{
    public class DetailsQ
    {
        public class Query : IRequest<Result<Model>>
        {
            public string Username { get; set; }
        }

        public class Model
        {
            public string Username { get; set; }
            public string Email { get; set; }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator ()
            {
                RuleFor (x => x.Username).NotEmpty ();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Model>>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;
            private readonly ILogger _logger;

            public Handler (BloggyContext context,
                IMapper mapper,
                ILogger<UsersController> logger)
            {
                _context = context;
                _mapper = mapper;
                _logger = logger;
            }

            protected override async Task<Result<Model>> HandleCore (Query message)
            {
                _logger.LogInformation ("{}", message.Username);
                var user = await SingleAsync (message.Username);
                _logger.LogInformation ("{}", user);

                if (user == null)
                    return Result.Fail<Model> ("User does not exit");

                var model = _mapper.Map<ApplicationUser, Model> (user);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> SingleAsync (string username)
            {
                return await _context.Users
                    .Where (u => u.Username == username)
                    .SingleOrDefaultAsync ();
            }
        }
    }
}
