using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Book
    {
        public Book()
        {

        }
        public int BookId { get; set; }
        public DateTime StartDate { get; set; }
        public int StartAt { get; set; }
        public int CustomerId { get; set; }
        public int ServiceId { get; set; }
        public int Durtion { get; set; }
        public int ServiceTypeId { get; set; }
        public string Notes { get; set; }
        public Guid? ArrivalToken { get; set; }
        public int ArrivalStatus { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual Service Service { get; set; }
        public virtual Servicetype ServiceType { get; set; }
    }
}
