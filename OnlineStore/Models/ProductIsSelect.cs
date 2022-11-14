namespace OnlineStore.Models
{
    public class ProductIsSelect: Product
    {
        public ProductIsSelect() : base() { }

        public bool IsSelect { get; set; }
        public ProductIsSelect(Product product)
        {
            this.Id = product.Id;
            this.Name = product.Name;
            this.Price = product.Price; 
            this.PriceSale = product.PriceSale; 
            this.Category = product.Category;
            this.Manufacturer = product.Manufacturer;
            this.InfoAboutProduct = product.InfoAboutProduct;
            this.imagesProduct = product.imagesProduct;
            this.IsSelect = false;
        }
    }
}
