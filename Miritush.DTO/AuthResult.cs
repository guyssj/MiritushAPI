using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class AuthResult
    {
        public string Access_token { get; set; }
        public DateTime ExpireIn { get; set; }
    }
}