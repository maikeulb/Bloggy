using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Bloggy.API.Features.Tags
{
    [Route ("api/tags")]
    public class TagsController : Controller
    {
        private readonly IMediator _mediator;

        public TagsController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> List ()
        {
            var result = await _mediator.Send (new List.Query query)

            return OK (result)
        }
    }
}
