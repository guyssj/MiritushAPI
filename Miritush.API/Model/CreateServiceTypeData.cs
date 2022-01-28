namespace Miritush.API.Model
{
    public class CreateServiceTypeData
    {
        public string Name { get; set; }
        public int ServiceId { get; set; }
        public int Duration { get; set; }
        public decimal Price { get; set; }
        public string Description { get; set; } = "";
    }
}