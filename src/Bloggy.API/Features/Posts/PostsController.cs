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
        public async Task<IActionResult> Details ([FromQuery] Details.Query query) 
        {
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
        public async Task<IActionResult> Edit ([FromBody] Edit.Command command)
        {
            await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }

        [HttpDelete ("{id}")]
        [Authorize (AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete ([FromQuery] Delete.Command command) 
        {
            await _mediator.Send (command);

            return result.IsSuccess
                ? (IActionResult)NoContent()
                : (IActionResult)BadRequest(result.Error);
        }
    }
}
