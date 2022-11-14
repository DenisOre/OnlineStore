namespace OnlineStore.Models
{
    public class OrderIsSelect: Order
    {
        public OrderIsSelect():base(){}

        public bool IsSelect { get; set; }

        public OrderIsSelect(Order order)
        {
            this.Id = order.Id;
            this.DateOrder = order.DateOrder;
            this.User = order.User;
            this.products = order.products;
            this.isReady = order.isReady;
            this.isPaid = order.isPaid;
            this.SumOfOrder = order.SumOfOrder;
            this.IsSelect = false;
        }
    }
}
