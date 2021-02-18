using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Ads;
using Application.Users;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        //Allow anyone to access this request
        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<User>> Login(Login.Query query)
        {
            return await _mediator.Send(query);
        }

        //Allow anyone to access this request
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult> RegisterCustomer(Register.Command command)
        {
            command.Origin = Request.Headers["origin"];
            await _mediator.Send(command);
            return Ok("Registration successful - please check your email");
        }

        //Allow anyone to access this request
        [AllowAnonymous]
        [HttpPost("verifyEmail")]
        public async Task<ActionResult> VerifyEmail(ConfirmEmail.Command command)
        {
            var result = await _mediator.Send(command);
            if(!result.Succeeded) return BadRequest("Problem verifying email address");
            return Ok("Email confirmed - you can now login");
        }

        //Allow anyone to access this request
        [AllowAnonymous]
        [HttpGet("resendEmailVerification")]
        public async Task<ActionResult> ResendEmailVerification([FromQuery]ResendEmailVerification.Query query)
        {
            query.Origin = Request.Headers["origin"];
            await _mediator.Send(query);
         
            return Ok("Email verification link sent - please check email");
        }

        //The user needs to be authenticated
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<User>> CurrentUser()
        {
            return await _mediator.Send(new CurrentUser.Query());
        }

        //The user needs to be authenticated
        [HttpGet("myAds")]
        [Authorize]
        public async Task<ActionResult<List<Ads>>> UserList()
        {
            return await _mediator.Send(new UserAds.Query());
        }

        //The user needs to be authenticated
        [HttpPost("edit")]
        [Authorize]
        public async Task<ActionResult<Unit>> EditProfile(EditProfile.Command command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("captcha")]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> VerifyCaptcha(VerifyCaptcha.Query query)
        {
            return await _mediator.Send(query);
        }
    }
}