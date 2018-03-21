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
        public async Task<IActionResult> List ([FromQuery] List.Query query)
        {
            var result = await _mediator.Send (query);

            return OK (result);
        }

        [HttpGet ("{id}", Name = "Details")]
        public async Task<IActionResult> Details ([FromRoute] int id)
        {
            var query = new Query.Details { Id = id };
            var result = await _mediator.Send (query);

            return result.IsSuccess
                ? (IActionResult)Ok()
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)CreatedAtRoute ("Details", new { controller = "Posts", id = result.Id }, result)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPut ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromRoute]int id, [FromBody] Edit.Command command)
        {
            command.Id = id;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpDelete ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Delete ([FromRoute]int id, [FromBody] Delete.Command command)
        {
            command.Id = id;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }
    }
}
