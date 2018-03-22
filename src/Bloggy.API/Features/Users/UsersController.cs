using System.Threading.Tasks;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services;
using Bloggy.API.Services.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Bloggy.API.Features.Auth;

namespace Bloggy.API.Features.Users
{
    [Route ("api/users")]
    public class UsersController: Controller
    {
        private readonly IMediator _mediator;
        private readonly ILogger _logger;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UsersController (
            IMediator mediator, 
            ICurrentUserAccessor currentUserAccessor,
            ILogger<UsersController> logger)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
            _logger = logger;
        }

        [HttpGet ("{username}")]
        public async Task<IActionResult> Details ([FromRoute] string username)
        {
            var query = new DetailsQ.Query { Username = username };

            var result = await _mediator.Send (query);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }

        [HttpGet]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Mine ()
        {
            var username  = _currentUserAccessor.GetCurrentUsername ();
            _logger.LogInformation("*********{}", username);
            var command = new DetailsQ.Query { Username = _currentUserAccessor.GetCurrentUsername () };
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }

        [HttpPut]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromBody] Edit.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) NoContent () :
                (IActionResult) BadRequest (result.Error);
        }
    }
}
