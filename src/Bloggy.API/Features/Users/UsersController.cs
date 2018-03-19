using System.Threading.Tasks;
using System.Threading.Tasks;
using Bloggy.API.Infrastructure.Interfaces;
using MediatR;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Users
{
    [Route ("api/users")]
    public class UsersController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UsersController (IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet ("{username}")]
        public async Task<IActionResult> Details ([FromBody] Details.Query query)
        {
            var result = await _mediator.Send (query);

            return OK (result)
        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Mine (Mine.Query query)
        {
            query.Username = _currentUserAccessor.GetCurrentUsername ()
            var result = await _mediator.Send (command);

            return NoContent (result);
        }

        [HttpPut]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromBody] Edit.Command command)
        {
            await _mediator.Send (command);

            return NoContent ();
        }
    }
}