using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using Application.Interface;
using Domain;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Persistence;

namespace Application.Ads
{
    public class Delete
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var ad = await _dataContext.Ads.FindAsync(request.Id);
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if(ad == null)
                    throw new RestException(HttpStatusCode.BadRequest, new {ad = "Problem finding your ad"});

                if(ad.AppUserId != user.Id)
                    throw new RestException(HttpStatusCode.Unauthorized, new {ad = "You are not allowed to delete this ad"});

                _dataContext.Ads.Remove(ad);

                if(await _dataContext.SaveChangesAsync() > 0)
                    return Unit.Value;
                
                throw new RestException(HttpStatusCode.BadRequest, new {ad = "Problem occurs while saving changes"});
            }
        }

    }


}