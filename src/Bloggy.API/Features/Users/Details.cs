using System.Net;
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
    public class Details
    {
        public class Query : IRequest<Model>
        {
            public class Model
            {
                public string Username { get; set; }
            }
        }

        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;
            private readonly IMapper _mapper;

            public QueryHandler(BloggyContext context, IMapper mapper)
            {
                _context = context;
                _mapper = mapper;
            }

            protected override async Task<Model> HandleCore (Query message, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == message.Username, cancellationToken);
                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                return new UserEnvelope(_mapper.Map<Domain.User, User>(user));
            }
        }
    }
}
