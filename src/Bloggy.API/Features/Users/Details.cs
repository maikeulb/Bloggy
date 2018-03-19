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
            public class Model
            {
                public string Username { get; set; }
            }
        }

        public class Validator : AbstractValidator<Command>
        {
            public Validator()
            {
                RuleFor(x => x.Username).NotNull().NotEmpty();
            }
        }

        public class Handler : AsyncRequestHandler<Query, Model>
        {
            private readonly BloggyContext _context;
            private readonly ICurrentUserAccessor _currentUserAccessor;

            public Handler(BloggyContext context, ICurrentUserAccessor currentUserAccessor)
            {
                _context = context;
                _currentUserAccessor = currentUserAccessor;
            }

            protected override async Task<Model> HandleCore (Query message, CancellationToken cancellationToken)
            {
                var currentUserName = _currentUserAccessor.GetCurrentUsername();

                var user = await _context.ApplicationUsers.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Username == username);

                if (user == null)
                {
                    throw new RestException(HttpStatusCode.NotFound);
                }
                var profile = _mapper.Map<Domain.ApplicationUsers, Model>(user);

                if (currentUserName != null)
                {
                    var currentUser = await _context.ApplicationUser
                        .FirstOrDefaultAsync(x => x.Username == currentUserName);
                }
                return profile
            }
        }
    }
}
