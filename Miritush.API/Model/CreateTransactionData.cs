using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateTransactionData
    {
        [Required]
        public int CustomerId { get; set; }
        public int? BookId { get; set; }
    }
}