using GonzaShoes.Model.Entities.Order;
using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Product
{
    [Table("ProductStockFlow")]
    public class ProductStockFlow : ContextualProps
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int ModelProductId { get; set; }
        public ModelProduct ModelProduct { get; set; } = null!;

        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;

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
