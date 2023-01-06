using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateTransactionItemData
    {
        [Required]
        public int TranscationId { get; set; }
        public int? ProductId { get; set; }
        public int? ServiceTypeId { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}