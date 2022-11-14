namespace OnlineStore.Models
{
    public class Order: IComparable<Order>
    {
        public int Id { get; set; }
        public DateTime DateOrder { get; set; }
        public User? User { get; set; }
        public List<Buy>? products { get; set; }
        public double? SumOfOrder { get; set; }
        public bool isRegistered { get; set; }
        public bool isReady { get; set; }
        public bool isPaid { get; set; }

        public int CompareTo(Order obj)
        {
            return DateOrder.CompareTo(obj.DateOrder);
        }

        public void CalcSumOfOrder()
        {
            SumOfOrder = 0;
            foreach (var item in products)
            {
                SumOfOrder += item.Count * item.buyPrice;
            }
        }
    }
}
