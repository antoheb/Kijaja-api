using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Application.Validators;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Users
{
    public class Register
    {
        public class Command : IRequest
        {
            public string FirstName { get; set; }

            public string LastName { get; set; }

            public string Email { get; set; }

            public string Password { get; set; }

            public string confirmPassword {get; set;}

            public string Origin { get; set; }
        }

        //Using fluent validation to ensure all the fields have validated data
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Password).Password();
                RuleFor(x => x.confirmPassword).NotEmpty().WithMessage("You must confirm your password");
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _context;

            private readonly UserManager<AppUser> _userManager;

            private readonly IEmailSender _emailSender;

            //Declaring constructor to have acess to our data context
            public Handler(DataContext context, UserManager<AppUser> userManager, IEmailSender emailSender)
            {
                _emailSender = emailSender;
                _userManager = userManager;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                //Ensure the user has the right password
                if(request.Password != request.confirmPassword)
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Your passwords do not correspond" });

                //Return an error if the username choosen already exist
                if (await _context.Users.AnyAsync(x => x.UserName == request.Email))
                    throw new RestException(HttpStatusCode.BadRequest, new { Username = "Email already registered" });

                var user = new AppUser
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    UserName = request.Email,
                    Email = request.Email,
                };

                var result = await _userManager.CreateAsync(user, request.Password);

                //  throw error if the new user creation request failed
                if (!result.Succeeded)
                    throw new RestException(HttpStatusCode.BadRequest, new { message = "Error while saving your information" });

                //Creation and encoding of the email token
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));

                //Creation of messsage to put in the email
                var verifyUrl = $"{request.Origin}/client/verifier-email?token={token}&email={request.Email}";
                var message = $"<p>Click on the link to confirm your email address</p><p><a href='{verifyUrl}'>{verifyUrl}</a></p>";

                //Use of external API called SendGrid to send the email
                await _emailSender.SendEmailAsync(request.Email, "Please confirm to verify you email", message);
                return Unit.Value;
            }
        }
    }
}