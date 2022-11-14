namespace OnlineStore.Models
{
    public class Buy: IComparable<Buy>
    {
        public int Id { get; set; }
        public Product? Product { get; set; }
        public int Count { get; set; } 
        public double buyPrice { get; set; }
        public double buySum { get; set; }
        public int? OrderId { get; set; }

        public int CompareTo(Buy buy)
        {
            return Product.Name.CompareTo(buy.Product.Name);
        }

        public void CalcBuySum()
        {
            buySum = Count * buyPrice;
        }
    }
}
