using System.Threading.Tasks;
using Application.Addresses;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class AddressController
    {
        private readonly IMediator _mediator;

        public AddressController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Unit>> Add(Add.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("edit")]
        [Authorize]
        public async Task<ActionResult<Unit>> Edit(Modify.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("delete")]
        [Authorize]
        public async Task<ActionResult<Unit>> Delete()
        {
            return await _mediator.Send(new Delete.Command());
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserAddress>> LoadAddress()
        {
            return await _mediator.Send(new LoadAddress.Query());
        }
    }
}