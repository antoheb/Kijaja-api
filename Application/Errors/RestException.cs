using System;
using System.Net;

namespace Application.Errors
{
    public class RestException : Exception
    {
        public readonly HttpStatusCode code;
        
        public object Errors { get; }

        public RestException(HttpStatusCode code, object errors = null)
        {
            this.code = code;
            this.Errors = errors;
        }
    }
}