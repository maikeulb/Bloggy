using System.Threading.Tasks;
using AutoMapper;
using Bloggy.API.Data;
using Bloggy.API.Entities;
using CSharpFunctionalExtensions;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

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

            public Handler (BloggyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Result<Model>> HandleCore (Query message)
            {
                var user = await SingleAsync (message.Username);

                if (user == null)
                    return Result.Fail<Model> ("User does not exit");

                var model = _mapper.Map<ApplicationUser, Model> (user);

                return Result.Ok (model);
            }

            private async Task<ApplicationUser> SingleAsync (string username)
            {
                return await _context.Users
                    .SingleOrDefaultAsync (u => u.Username == username);
            }
        }
    }
}
