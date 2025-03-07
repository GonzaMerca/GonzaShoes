namespace GonzaShoes.Model.DTOs.Order
{
    public class OrderItemDTO : ContextualProps
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public string ProductName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        public int ModelProductId { get; set; }
        public string ModelProductName { get; set; } = string.Empty;

        public int ColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;

        public int SizeId { get; set; }
        public decimal SizeNumber { get; set; }

        public decimal Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalWithNoDiscount { get; set; }
        public decimal Discount { get; set; }
        public decimal DiscountPercentage { get; set; }
        public decimal Total { get; set; }
        public string Observation { get; set; } = string.Empty;
    }
}
