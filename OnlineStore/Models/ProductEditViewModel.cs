namespace OnlineStore.Models
{
    public class ProductEditViewModel
    {
        public Product Product { get; set; }
        public List<Category> Categories { get; set; }
        public List<Manufacturer> Manufacturers { get; set; }
        public List<ImageProduct> ImageProducts { get; set; }
    }
}
