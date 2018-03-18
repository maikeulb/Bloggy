using System.Threading.Tasks;
using Bloggy.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.Features.Comments
{
    [Route("api/posts/{postId}/comments")]
    public class CommentsController : Controller
    {
        private readonly IMediator _mediator;

        public CommentsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetComments([FromQuery] GetPost.Query query)
        {
            result = await _mediator.Send(query)

            return OK() 
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtIssuerOptions.Schemes)]
        public async Task<IActionResult> Create([FromBody]Create.Command command)
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
