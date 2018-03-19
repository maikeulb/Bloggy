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

        [HttpGet("List")]
        public async Task<IActionResult> List([FromQuery] GetPost.Query query) // suport filtering by tag and category; include comments
        {
            var result = await _mediator.Send(query)

            return OK(result) 
        }

        [HttpGet("Mine")]
        public async Task<IActionResult> Mine([FromQuery] GetPosts.Query query) // include comments
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
        public async Task Delete([FromQuery]Create.Command command) //cascade delete comments
        {
            await _mediator.Send(command);

            return NoContent ();
        }
    }
}
