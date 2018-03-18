using System.Threading.Tasks;
using Bloggy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Features.Posts
{
    [Route("api/[Controller]")]
    public class PostsController : Controller
    {
        private readonly IMediator _mediator;

        public PostsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPost([FromQuery] GetPost.Query query)
        {
            result = await _mediator.Send(query)

            return OK() 
        }

        [HttpGet]
        public async Task<IActionResult> GetPosts([FromQuery] GetPosts.Query query)
        {
            result = await _mediator.Send(query)

            return OK() 
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<PostEnvelope> Create([FromBody]Create.Command command)
        {
            await _mediator.Send(command);

            return NoContent ();
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<PostEnvelope> Edit(string id, [FromBody]Edit.Command command)
        {
            command.Id = id;
            await _mediator.Send(command);

            return NoContent ();
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task Delete(string id)
        {
            await _mediator.Send(new Delete.Command(id));

            return NoContent ();
        }
    }
}
