using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using Application.Interface;

namespace Application.Users
{
    public class VerifyCaptcha
    {
        public class Query : IRequest<Boolean>
        {
            public string Token { get; set; }
        }

        public class CommandValidator : AbstractValidator<Query>
        {
            public CommandValidator()
            {
                // Make sure we can't pass an empty token to the backend.
                RuleFor(x => x.Token).NotEmpty().WithMessage("Token cannot be emtpy");
            }
        }

        public class Handler : IRequestHandler<Query, Boolean>
        {
            private readonly ICaptchaVerificator _verificator;
            public Handler(ICaptchaVerificator verificator)
            {
                _verificator = verificator;
            }
            public async Task<bool> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _verificator.VerifyToken(request.Token);
            }
        }
    }
}