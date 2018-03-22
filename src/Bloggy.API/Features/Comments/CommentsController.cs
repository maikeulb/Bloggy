using System.Threading.Tasks;
using Bloggy.API.Infrastructure;
using Bloggy.API.Services.Interfaces;
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
        public async Task<IActionResult> ListAll ([FromRoute] int postId)
        {
            var query = new ListAll.Query { PostId = postId };
            var result = await _mediator.Send (query);

            return result.IsSuccess
                ? (IActionResult)Ok(result.Value)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpGet ("{id}", Name = "CommentDetails")]
        public async Task<IActionResult> Details ([FromRoute] int postId, [FromRoute] int id)
        {
            var query = new DetailsQ.Query { PostId = postId, Id = id };
            var result = await _mediator.Send (query);

            return result.IsSuccess
                ? (IActionResult)Ok(result.Value)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPost]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create ([FromRoute] int postId, [FromBody] Create.Command command)
        {
            command.PostId = postId;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)CreatedAtRoute ("CommentDetails", new { controller = "Comments", postId = command.PostId, id = result.Value.Id }, result)
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpPut ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit ([FromRoute] int postId, [FromQuery] int id, [FromBody] Edit.Command command)
        {
            command.PostId = postId;
            command.Id = id;
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpDelete ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Delete ([FromRoute] int postId, [FromRoute] int id)
        {
            var command = new Delete.Command { PostId = postId, Id = id };
            var result = await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }
    }
}
