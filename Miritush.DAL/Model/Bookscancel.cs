﻿using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Bookscancel
    {
        public int BookId { get; set; }
        public int CustomerId { get; set; }
        public int Durtion { get; set; }
        public string Notes { get; set; }
        public int? ServiceId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int StartAt { get; set; }
        public DateTime StartDate { get; set; }
        public string WhyCancel { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }
        public virtual Servicetype ServiceType { get; set; }
    }
}
