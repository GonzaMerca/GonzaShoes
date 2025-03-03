namespace GonzaShoes.Model.Entities.Order
{
    public class Order : ContextualProps
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public OrderPayment OrderPayment { get; set; } = null!;
        public List<OrderItem> OrderItems { get; set; } = [];
        public decimal TotalWithNoDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Total { get; set; }
        public string Observation { get; set; } = string.Empty;
    }
}
