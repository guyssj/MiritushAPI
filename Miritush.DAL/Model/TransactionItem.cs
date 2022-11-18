using System;

namespace Miritush.DAL.Model
{
    public partial class TransactionItem
    {
        public int Id { get; set; }
        public int TranscationId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }

        public virtual Transaction Transaction { get; set; }
        public virtual Product Product { get; set; }
        public virtual Servicetype ServiceType { get; set; }




    }
}