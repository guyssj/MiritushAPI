using System;
using System.Collections.Generic;

#nullable disable

namespace Miritush.DAL.Model
{
    public partial class Customer
    {
        public Customer()
        {
            Books = new HashSet<Book>();
            Attachments = new HashSet<Attachment>();

        }

        public int CustomerId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Color { get; set; }
        public string Notes { get; set; }
        public int? Otp { get; set; }
        public byte? Active { get; set; }

        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<Attachment> Attachments { get; set; }

    }
}
