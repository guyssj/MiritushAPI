using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class ErrorMessageResult
    { 
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public string StackTrace { get; set; }

    }
}
