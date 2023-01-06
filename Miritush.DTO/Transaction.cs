using System;
using System.Collections.Generic;
using Miritush.DTO.Enums;

namespace Miritush.DTO
{
    public class Transaction
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        public TransactionStatus Status { get; set; }
        public List<DTO.TransactionItem> Items { get; set; }

    }
}