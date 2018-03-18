using System.Threading.Tasks;
using Bloggy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Posts
{
    [Route("api/posts")]
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("GetAllPosts")]
        public async Task<IActionResult> GetAllPosts([FromQuery] GetPost.Query query)
        {
            var result = await _mediator.Send(query)

            return OK(result) 
        }

        [HttpGet("GetUserPosts")]
        public async Task<IActionResult> GetUserPosts([FromQuery] GetPosts.Query query)
        {
            var result = await _mediator.Send(query)

            return OK(result) 
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create([FromBody]Create.Command command)
        {
            await _mediator.Send(command);

            return NoContent ();
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Edit(string id, [FromBody]Edit.Command command)
        {
            command.Id = id;
            await _mediator.Send(command);

            return NoContent ();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete([FromQuery]Create.Command command)
        {
            await _mediator.Send(command);

            return NoContent ();
        }
    }
}
