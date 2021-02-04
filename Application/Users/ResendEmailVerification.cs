using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Application.Users
{
    public class ResendEmailVerification
    {
        public class Query : IRequest
        {
            public string Email { get; set; }

            public string Origin { get; set; }
        }
        public class Handler : IRequestHandler<Query>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IEmailSender _emailSender;
            public Handler(UserManager<AppUser> userManager, IEmailSender emailSender)
            {
                _emailSender = emailSender;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByEmailAsync(request.Email);

                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                var verifyUrl = $"{request.Origin}/client/verifier-email?token={token}&email={request.Email}";
                var message = $"<p>Click on the link to confirm your email address</p><p><a href='{verifyUrl}'>{verifyUrl}</a></p>";

                await _emailSender.SendEmailAsync(request.Email, "Please confirm to verify you email", message);
                return Unit.Value;
            }
        }
    }
}