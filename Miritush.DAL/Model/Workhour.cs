using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Workhour
    {
        public int DayOfWeek { get; set; }
        public int OpenTime { get; set; }
        public int CloseTime { get; set; }
    }
}
