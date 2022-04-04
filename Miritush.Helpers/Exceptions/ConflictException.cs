using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Miritush.Helpers.Exceptions
{
    public class ConflictException : Exception
    {
        public HttpStatusCode Code { get; } = HttpStatusCode.Conflict;
        public ConflictException() : base() { }

        public ConflictException(string message) : base(message) { }

        public ConflictException(string message, params object[] args)
            : base(String.Format(CultureInfo.CurrentCulture, message, args))
        {
        }
    }
}