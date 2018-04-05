using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Auth
{
    [Route ("auth")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost ("Register")]
        public async Task<IActionResult> Register ([FromBody] Register.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> Login ([FromBody] Login.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }
    }
}
