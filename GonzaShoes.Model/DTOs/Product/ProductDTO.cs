using GonzaShoes.Model.DTOs.Color;
using GonzaShoes.Model.DTOs.ModelProduct;
using GonzaShoes.Model.DTOs.Size;

namespace GonzaShoes.Model.DTOs.Product
{
    public class ProductDTO : ContextualProps
    {
        public int Id { get; set; }
        //TODO: Ver de utilizar marca + model + color + talle
        //public string Name { get; set; } = string.Empty;

        // Relationship with Model
        public int ModelId { get; set; }
        public ModelProductDTO Model { get; set; } = null!;

        // Relationship with Color
        public int ColorId { get; set; }
        public ColorDTO Color { get; set; } = null!;

        // Relationship with Size
        public int SizeId { get; set; }
        public SizeDTO Size { get; set; } = null!;

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
