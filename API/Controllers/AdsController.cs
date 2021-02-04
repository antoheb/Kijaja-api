using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Ads;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AdsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AdsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Unit>> Create(Create.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<ActionResult<Unit>> Edit(Guid id, Edit.Command command)
        {
            command.AdId = id;
            return await _mediator.Send(command);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Unit>> Delete(Guid id, Delete.Command command)
        {
            command.AdId = id;
            return await _mediator.Send(command);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<Ads>>> List()
        {
            return await _mediator.Send(new List.Query());
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Ads>> Details(Guid id)
        {
            return await _mediator.Send(new Details.Query{AdId = id});
        }
    }
}