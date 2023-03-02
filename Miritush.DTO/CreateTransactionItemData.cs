using System.ComponentModel.DataAnnotations;

namespace Miritush.DTO
{
    public class CreateTransactionItemData
    {
        [Required]
        public int TransactionId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}