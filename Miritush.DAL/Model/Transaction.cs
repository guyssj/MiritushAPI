using System;
using System.Collections.Generic;

namespace Miritush.DAL.Model
{
    public partial class Transaction
    {
        public Transaction()
        {
            TransactionItems = new HashSet<TransactionItem>();
        }
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public byte Status { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<TransactionItem> TransactionItems { get; set; }

    }
}