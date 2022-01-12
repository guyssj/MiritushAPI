using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Closeday
    {
        public int CloseDaysId { get; set; }
        public DateTime Date { get; set; }
        public string Notes { get; set; }
    }
}
