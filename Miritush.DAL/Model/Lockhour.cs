using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Lockhour
    {
        public int IdLockHours { get; set; }
        public DateTime StartDate { get; set; }
        public int StartAt { get; set; }
        public int EndAt { get; set; }
        public string Notes { get; set; }
    }
}
