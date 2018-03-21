using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace Bloggy.API.Features.Categories
{
    [Route ("api/categories")]
    public class CategoriesController : Controller
    {
        private readonly IMediator _mediator;

        public CategoriesController (IMediator mediator)
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
