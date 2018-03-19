using System.Threading.Tasks;
using Bloggy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Comments
{
    [Route ("api/posts/{postId}/comments")]
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;

        public CommentsController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List ([FromQuery] List.Query query)
        {
            result = await _mediator.Send (query)

            return OK (result)
        }

        [HttpGet ("{id}", Name = "Details")]
        public async Task<IActionResult> Details ([FromQuery] Detials.Query query)
        {
            result = await _mediator.Send (query)

            return OK (result)
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            result = await _mediator.Send (command)

            return CreatedAtRoute ("Details", new { controller = "Comments", id = result.Id }, result);
        }

        [HttpPut ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromBody] Edit.Command command)
        {
            await _mediator.Send (command);

            return NoContent ();
        }

        [HttpDelete ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete ([FromQuery] Delete.Command command)
        {
            await _mediator.Send (command);

            return NoContent ();
        }
    }
}