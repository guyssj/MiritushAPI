using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Miritush.DTO
{
    public class LockHour
    {
        public int IdLockHours { get; set; }
        public DateTime StartDate { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public string Notes { get; set; }
    }
}