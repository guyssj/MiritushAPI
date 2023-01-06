namespace Miritush.DTO
{
    public class TransactionItem
    {

        public int Id { get; set; }
        public int TranscationId { get; set; }
        public int? ProductId { get; set; }
        public string ProductName { get; set; }
        public int? ServiceTypeId { get; set; }
        public string ServiceTypeName { get; set; }
        public int Quantity { get; set; }
        public decimal? Price { get; set; }
    }
}