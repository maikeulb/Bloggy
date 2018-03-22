using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Bloggy.API.Features.Auth
{
    [Route ("api/auth")]
    public class AuthController : Controller
    {
        private readonly IMediator _mediator;

        public AuthController (IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost ("Create")]
        public async Task<IActionResult> Create ([FromBody] Create.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }

        [HttpPost ("Login")]
        public async Task<IActionResult> Login ([FromBody] Login.Command command)
        {
            var result = await _mediator.Send (command);

            return result.IsSuccess ?
                (IActionResult) Ok (result.Value) :
                (IActionResult) BadRequest (result.Error);
        }
    }
}
