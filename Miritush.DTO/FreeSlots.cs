using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class FreeSlots
    {
        public DateTime startDate { get; set; }
        public int startAt { get; set; }
        public int endAt { get; set; }

        public FreeSlots(DateTime startDate, int startAt, int endAt)
        {
            this.startDate = startDate;
            this.startAt = startAt;
            this.endAt = endAt;
        }
        public FreeSlots()
        {

        }
    }

}