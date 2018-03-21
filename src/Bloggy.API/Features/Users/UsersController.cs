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
        public async Task<IActionResult> Details ([FromRoute] string username)
        {
            var query = new Details.Query { Username = username }; 
            var result = await _mediator.Send (query);

            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Mine ()
        {
            var command = new Details.Query { Username = _currentUserAccessor.GetCurrentUsername ()};
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)Ok(result)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPut]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromBody] Edit.Command command)
        {
            await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent(result)
                : (IActionResult)BadRequest(result.Error);
        }
    }
}
