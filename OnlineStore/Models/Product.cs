namespace OnlineStore.Models
{
    public class Product: IComparable<Product>
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public double Price { get; set; }
        public double PriceSale { get; set; }
        public Category? Category { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public string? InfoAboutProduct { get; set; }
        public List<ImageProduct>? imagesProduct { get; set; }

        public int CompareTo(Product obj)
        {
            return Name.CompareTo(obj.Name);
        }
    }
}
