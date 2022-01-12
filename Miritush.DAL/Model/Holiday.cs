using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Holiday
    {
        public int HolidayId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
