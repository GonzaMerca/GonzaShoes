using GonzaShoes.Model.DTOs.Size;

namespace GonzaShoes.Model.DTOs.Product
{
    public class ProductDTO : ContextualProps
    {
        public int Id { get; set; }
        //TODO: Ver de utilizar marca + model + color + talle
        //public string Name { get; set; } = string.Empty;

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

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
