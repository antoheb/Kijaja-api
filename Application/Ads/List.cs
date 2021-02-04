using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Ads
{
    public class List
    {
        public class Query : IRequest<List<Ads>> {}

        public class Handler : IRequestHandler<Query, List<Ads>>
        {
            private readonly DataContext _dataContext;
            
            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }
            
            public async Task<List<Ads>> Handle(Query request, CancellationToken cancellationToken)
            {
                var list = await _dataContext.Ads.ToListAsync();
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