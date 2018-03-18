using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Conduit.Infrastructure;
using Conduit.Infrastructure.Security;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Bloggy.API.Features.Users
{
    [Route("users")]
    public class UsersController
    {
        private readonly IMediator _mediator;
        private readonly ICurrentUserAccessor _currentUserAccessor;

        public UsersController(IMediator mediator, ICurrentUserAccessor currentUserAccessor)
        {
            _mediator = mediator;
            _currentUserAccessor = currentUserAccessor;
        }

        [HttpGet("{username}")]
        public async Task<IActionResult> GetUser([FromBody] GetUser.Query query)
        {
            query.Username = _currentUserAccessor.GetCurrentUsername()
            return await _mediator.Send(query);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> GetCurrent()
        {
            return await _mediator.Send(command);

        }

        [HttpPut]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> UpdateUser([FromBody]Edit.Command command)
        {
            return await _mediator.Send(command);
        }
    }
}
