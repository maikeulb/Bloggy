using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Auth
{
    [Route ("api/auth")]
    public class AuthController
    {
        private readonly IMediator _mediator;

        public AuthController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost ("Create")]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            var resultOrError = await _mediator.Send (command);

            return resultOrError.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(modelOrError.Error);
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> Login ([FromBody] Login.Command command)
        {
            var resultOrError = await _mediator.Send (command);

            return resultOrError.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)Unauthorized(modelOrError.Error));
        }
    }
}
