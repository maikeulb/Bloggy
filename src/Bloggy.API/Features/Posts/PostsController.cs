using System.Threading.Tasks;
using MediatR;
using Bloggy.API.Infrastructure;
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
        public async Task<IActionResult> ListAll ([FromQuery] ListAll.Query query)
        {
            var result = await _mediator.Send (query);

            return Ok (result);
        }

        [HttpGet ("{postId}", Name = "PostDetails")]
        public async Task<IActionResult> Details ([FromRoute] int postId)
        {
            var query = new Details.Query { Id = postId };
            var result = await _mediator.Send (query);

            return result.IsSuccess
                ? (IActionResult)Ok(result.Value)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)CreatedAtRoute ("PostDetails", new { controller = "Posts", postId = result.Value.Id }, result)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPut ("{postId}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromRoute]int postId, [FromBody] Edit.Command command)
        {
            command.Id = postId;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpDelete ("{postId}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Delete ([FromRoute]int postId, [FromBody] Delete.Command command)
        {
            command.Id = postId;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }
    }
}
