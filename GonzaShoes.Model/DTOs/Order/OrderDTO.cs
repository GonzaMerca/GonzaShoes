namespace GonzaShoes.Model.DTOs.Order
{
    public class OrderDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public DateTime DateTime { get; set; } = DateTime.Now;
        public OrderStatusEnum Status { get; set; } = OrderStatusEnum.Pending;
        public OrderPaymentDTO OrderPayment { get; set; } = null!;
        public List<OrderItemDTO> OrderItems { get; set; } = [];
        public decimal TotalWithNoDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Total { get; set; }
        public string Observation { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
