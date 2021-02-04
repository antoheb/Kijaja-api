using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Domain;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Addresses
{
    public class Add
    {
        public class Command : IRequest
        {
            public string Country { get; set; }

            public string Province { get; set; }

            public string Street { get; set; }

            public string City { get; set; }

            public string PostalCode { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Country).NotEmpty();
                RuleFor(x => x.Province).NotEmpty();
                RuleFor(x => x.Street).NotEmpty();
                RuleFor(x => x.City).NotEmpty();
                RuleFor(x => x.PostalCode).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            private readonly UserManager<AppUser> _userManager;
            
            private readonly IUserAccessor _userAccessor;

            public Handler(DataContext dataContext, UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _userManager = userManager;
                _dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                var address = new Address 
                {
                    Country = request.Country,
                    Province = request.Province,
                    Street = request.Street,
                    City = request.City,
                    PostalCode = request.PostalCode,
                    AppUserId = user.Id
                };

                _dataContext.Addresses.Add(address);

                if(await _dataContext.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new RestException(HttpStatusCode.BadRequest, new {message = "Problem occurs while saving your address information"});
            }
        }
    }
}