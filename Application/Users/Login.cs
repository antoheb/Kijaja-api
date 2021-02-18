using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Users
{
    public class Login
    {
        public class Query : IRequest<User>
        {
            public string Username { get; set; }
            public string Password { get; set; }
        }

        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.Username).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, User>
        {
            private readonly UserManager<AppUser> _userManager;
            public readonly SignInManager<AppUser> _signInManager;
            private readonly IJwtGenerator _jwtGenerator;
            public Handler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _signInManager = signInManager;
                _userManager = userManager;
            }

            public async Task<User> Handle(Query request, CancellationToken cancellationToken)
            {
                //Verify username exist in the database
                var user = await _userManager.FindByNameAsync(request.Username);

                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { message = "Email or password invalid" });

                //Verify if the email has been confirm, if not throw error
                if (!user.EmailConfirmed) throw new RestException(HttpStatusCode.BadRequest, new { Email = "Email address has not been confirm - go see your mail box" });

                //If the credentials are correct, the result return succeeded
                var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);

                if (!result.Succeeded)
                {
                    throw new RestException(HttpStatusCode.Unauthorized, new { message = "Email or password invalid" });
                }

                return new User
                {
                    Username = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }
        }
    }
}