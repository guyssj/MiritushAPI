using System;
using System.Globalization;
using System.Net;

namespace Miritush.Helpers.Exceptions
{
    public class NotFoundException : Exception
    {
        public HttpStatusCode Code { get; } = HttpStatusCode.NotFound;
        public NotFoundException() : base() { }

        public NotFoundException(string message) : base(message) { }

        public NotFoundException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}
