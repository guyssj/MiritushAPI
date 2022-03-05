using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.API.Model
{
    public class UpdateBookData
    {
        public int BookId { get; set; }
        public DateTime StartDate { get; set; }
        public int CustomerId { get; set; }
        public int StartAt { get; set; }
        public string Notes { get; set; }
    }
}