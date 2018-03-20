using AutoMapper;
using CSharpFunctionalExtensions;
using System.Threading;
using System.Threading.Tasks;
using Bloggy.API.Entities;
using Bloggy.API.Infrastructure;
using Bloggy.API.Infrastructure.Interfaces;
using Bloggy.API.Data;
using FluentValidation;
using MediatR;

namespace Bloggy.API.Features.Profiles
{
    public class Details
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

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Result<Model>>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Result<Model>> HandleCore(Query message)
            {
                var user = await SingleAsync (Username);

                if (user == null)
                    return Result.Fail<Model> ("User does not exit");

                var model = _mapper.Map<Entities.ApplicationUsers, Model>(user);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> SingleAsync(string username)
            {
                return await _context.ApplicationUsers
                    .Where(au => au.Username == username)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
