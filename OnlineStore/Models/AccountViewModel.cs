namespace OnlineStore.Models
{
    public class AccountViewModel
    {
        public List<Order> readyOrders { get; set; }
        public List<Order> inWorkOrders { get; set; }

        public List<Order> paidOrders { get; set; }
    }
}
