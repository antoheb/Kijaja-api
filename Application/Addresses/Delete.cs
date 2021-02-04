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
    public class Delete
    {
        public class Command : IRequest { }

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

                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { user = "No user found" });

                var address = _dataContext.Addresses.FirstOrDefault(x => x.AppUserId == user.Id);
                _dataContext.Addresses.Remove(address);

                if (await _dataContext.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new RestException(HttpStatusCode.BadRequest, new { message = "Problem occurs while saving your address information" });
            }
        }
    }
}