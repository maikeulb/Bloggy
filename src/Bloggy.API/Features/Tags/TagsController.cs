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
        public async Task<IActionResult> ListAll ()
        {
            var query = new ListAllQ.Query();
            var result = await _mediator.Send (query);

            return Ok (result);
        }
    }
}
