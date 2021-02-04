using System.Collections.Generic;
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

namespace Application.Ads
{
    public class UserAds
    {
        public class Query : IRequest<List<Ads>> { }

        public class Handler : IRequestHandler<Query, List<Ads>>
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

            public async Task<List<Ads>> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _userManager.FindByNameAsync(_userAccessor.GetCurrentUsername());

                if (user == null)
                    throw new RestException(HttpStatusCode.BadRequest, new { user = "User not found" });

                var list = _dataContext.Ads.Where(x => x.AppUserId == user.Id);
                var adsList = new List<Ads>();

                foreach (var ads in list)
                {
                    var newAds = new Ads
                    {
                        Name = ads.Name,
                        Description = ads.Description,
                        Picture = ads.Picture,
                        Price = ads.Price,
                        Category = ads.Category,
                        Status = ads.Status
                    };
                    adsList.Add(newAds);
                }

                return adsList;
            }
        }
    }
}