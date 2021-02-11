using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Errors;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.Ads
{
    public class Edit
    {
        public class Command : IRequest
        {
            public Guid AdId { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public double Price { get; set; }

            public string Category { get; set; }

            public string Picture { get; set; }

            public string Status { get; set; }
        }

        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.Description).NotEmpty();
                RuleFor(x => x.Price).NotEmpty();
                RuleFor(x => x.Category).NotEmpty();
                RuleFor(x => x.Status).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly DataContext _dataContext;

            public Handler(DataContext dataContext)
            {
                _dataContext = dataContext;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var ad = await _dataContext.Ads.FindAsync(request.AdId);

                if(ad == null)
                    throw new RestException(HttpStatusCode.BadRequest, new {ad = "Problem occurs when trying to find your ad"});

                ad.Name = request.Name ?? ad.Name;
                ad.Description = request.Description ?? ad.Description;
                ad.Price = request.Price;
                ad.Category = request.Category ?? ad.Category;
                ad.Picture = request.Picture ?? ad.Picture;
                ad.Status = request.Status ?? ad.Status;

                if(await _dataContext.SaveChangesAsync() > 0)
                    return Unit.Value;

                throw new RestException(HttpStatusCode.BadRequest, new {message = "Problem occurs while saving your ad information"});
            }
        }

    }
}