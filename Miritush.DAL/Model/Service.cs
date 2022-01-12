using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Service
    {
        public Service()
        {
            Books = new HashSet<Book>();
            Servicetypes = new HashSet<Servicetype>();
        }

        public int ServiceId { get; set; }
        public string ServiceName { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Servicetype> Servicetypes { get; set; }
    }
}
