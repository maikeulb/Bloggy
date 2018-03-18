using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Tags
{
    [Route("tags")]
    public class TagsController : Controller
    {
        private readonly IMediator _mediator;

        public TagsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetTags()
        {
            return await _mediator.Send(new GetCommands.Query query)
        }
    }
}
