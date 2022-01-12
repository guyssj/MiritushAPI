using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Servicetype
    {
        public Servicetype()
        {
            Books = new HashSet<Book>();
        }

        public int ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int ServiceId { get; set; }
        public int? Duration { get; set; }
        public decimal? Price { get; set; }
        public string Description { get; set; }

        public virtual Service Service { get; set; }
        public virtual ICollection<Book> Books { get; set; }
    }
}
