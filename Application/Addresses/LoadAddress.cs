using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Addresses
{
    public class LoadAddress
    {
        public class Query : IRequest<UserAddress> { }

        public class Handler : IRequestHandler<Query, UserAddress>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IUserAccessor _userAccessor;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext, IJwtGenerator jwtGenerator, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                _dataContext = dataContext;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _userManager = userManager;
            }

            public async Task<UserAddress> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());
                var address = _dataContext.Addresses.FirstOrDefault(x => x.AppUserId == user.Id);

                if(address == null)
                    return new UserAddress{};

                return new UserAddress 
                {
                    Country = address.Country,
                    PostalCode = address.PostalCode,
                    Province = address.Province,
                    Street = address.Street,
                    City = address.City
                };
            }
        }
    }
}