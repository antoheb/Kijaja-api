using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using MediatR;
using Persistence;

namespace Application.Ads
{
    public class Details
    {
        public class Query : IRequest<Ads>
        {
            public Guid AdId { get; set; }
        }

        public class Handler : IRequestHandler<Query, Ads>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                this._dataContext = dataContext;
            }

            public async Task<Ads> Handle(Query request, CancellationToken cancellationToken)
            {
                var ad = await _dataContext.Ads.FindAsync(request.AdId);

                if(ad == null)
                    throw new RestException(HttpStatusCode.NotFound, new {ad = "Error, Ad not found"});
                
                var ads = new Ads 
                {
                    Name = ad.Name,
                    Description = ad.Description,
                    Price = ad.Price,
                    Category = ad.Category,
                    Picture = ad.Picture,
                    Status = ad.Status
                };

                return ads;
            }
        }
    }
}