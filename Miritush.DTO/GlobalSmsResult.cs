using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class GlobalSmsResult
    {
        public bool Success { get; set; }
        public int Result { get; set; }
        public string ResultJSON { get; set; }
        public string ErrDesc { get; set; }

    }
}