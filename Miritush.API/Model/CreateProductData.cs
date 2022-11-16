using System.ComponentModel.DataAnnotations;

namespace Miritush.API.Model
{
    public class CreateProductData
    {
        [Required]
        public int CategoryID { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; } = 0;
        public bool Active { get; set; } = true;
    }
}