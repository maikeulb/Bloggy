using AutoMapper;
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
        public class Query : IRequest<Model>
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

        public class Handler : AsyncRequestHandler<Query, Result>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;
            private readonly IMapper _mapper;

            public Handler(BloggyContext context, ICurrentUserAccessor currentUserAccessor, IMapper mapper)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
                _mapper = mapper;
            }

            protected override async Task<Result> HandleCore(Query message)
            {
                var currentUsername = _currentUserAccessor.GetCurrentUsername();

                var user = await SingleAsync (currentUsername);

                if (user == null)
                    return Result.Fail<Model> ("User does not exit");

                var model = _mapper.Map<Entities.ApplicationUsers, Model>(user);

                return Result.Ok (model);
            }

            private async Task<Post> SingleAsync(int username)
            {
                return await _context.ApplicationUsers
                    .Where(au => au.Username == username)
                    .SingleOrDefaultAsync();
            }
        }
    }
}
