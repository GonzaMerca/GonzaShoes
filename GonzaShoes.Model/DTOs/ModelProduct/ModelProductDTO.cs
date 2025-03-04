using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.DTOs.Product;

namespace GonzaShoes.Model.DTOs.ModelProduct
{
    public class ModelProductDTO : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relationship with Brand
        public int BrandId { get; set; }
        public BrandDTO Brand { get; set; } = null!;

        // A model has many products
        public List<ProductDTO> Products { get; set; } = [];

    }
}
