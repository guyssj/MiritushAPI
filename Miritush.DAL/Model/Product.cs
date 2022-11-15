namespace Miritush.DAL.Model
{
    public partial class Product
    {
        public int Id { get; set; }
        public int CategoryID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public bool Active { get; set; } = true;

        public virtual ProductCategory Category { get; set; }
    }
}