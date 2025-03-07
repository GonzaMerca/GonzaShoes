using GonzaShoes.Model.Entities.Order;

namespace GonzaShoes.Model.DTOs.Product
{
    public class ProductStockFlowDTO : ContextualProps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string Name => $"{BrandName} - {ModelProductName}".Trim();

        // Relationship with Model
        public int ModelProductId { get; set; }
        public string ModelProductName { get; set; } = string.Empty;

        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        // Relationship with Color
        public int ColorId { get; set; }
        public string ColorName { get; set; } = string.Empty;
        public string ColorHexCode { get; set; } = string.Empty;

        // Relationship with Size
        public int SizeId { get; set; }
        public decimal SizeNumber { get; set; }

        public int? OrderId { get; set; }
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }
        public decimal RemainingStock { get; set; }
        public int? OrderProductItemId { get; set; }
        public OrderItem OrderProductItem { get; set; }
        public string Description { get; set; }

        public DateTime Date { get; set; }
    }
}
