using GonzaShoes.Model.Entities.Product;
using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Order
{
    [Table("OrderItem")]
    public class OrderItem : ContextualProps
    {
        public int Id { get; set; }
        public int OrderId { get; set; }

        public int ProductId { get; set; }
        public Product.Product Product { get; set; } = null!;
        public string ProductName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;
        public string BrandName { get; set; } = string.Empty;

        public int ModelProductId { get; set; }
        public ModelProduct ModelProduct { get; set; } = null!;
        public string ModelProductName { get; set; } = string.Empty;

        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;
        public string ColorName { get; set; } = string.Empty;

        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;
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
