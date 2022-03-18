using System;
using System.Globalization;
using System.Net;

namespace Miritush.Helpers.Exceptions
{
    public class UnauthorizedException : Exception
    {
        public HttpStatusCode Code { get; } = HttpStatusCode.Unauthorized;
        public UnauthorizedException() : base() { }

        public UnauthorizedException(string message) : base(message) { }

        public UnauthorizedException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
