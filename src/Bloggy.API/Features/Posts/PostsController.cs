using System.Threading.Tasks;
using Bloggy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Posts
{
    [Route ("api/posts")]
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;

        public PostsController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List ([FromQuery] List.Query query) // suport filtering by tag and category; include comments
        {
            var result = await _mediator.Send (query)

            return OK (result)
        }

        [HttpGet ("Mine")]
        public async Task<IActionResult> Mine ([FromQuery] Mine.Query query) // include comments
        {
            var result = await _mediator.Send (query)

            return OK (result)
        }

        [HttpGet ("{id}", Name = "Details")]
        public async Task<IActionResult> Details ([FromQuery] Details.Query query) // suport filtering by tag and category; include comments
        {
            var result = await _mediator.Send (query)

            return OK (result)
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            var result = await _mediator.Send (command)

            return CreatedAtRoute ("Details", new { controller = "Posts", id = result.Id }, result);
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
        public async Task Delete ([FromQuery] Delete.Command command) //cascade delete comments
        {
            await _mediator.Send (command);

            return NoContent ();
        }
    }
}
