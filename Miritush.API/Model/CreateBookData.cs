using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.API.Model
{
    public class CreateBookData
    {
        public DateTime StartDate { get; set; }
        public int StartAt { get; set; }
        public int CustomerId { get; set; }
        public List<int> ServiceTypeIds { get; set; }
        public int Duration { get; set; }
    }
}