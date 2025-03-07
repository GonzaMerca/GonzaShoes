using GonzaShoes.Model.DTOs.Brand;
using GonzaShoes.Model.DTOs.Product;

namespace GonzaShoes.Model.DTOs.ModelProduct
{
    public class ModelProductDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relationship with Brand
        public int BrandId { get; set; }
        public string BrandName { get; set; } = string.Empty;

        // A model has many products
        public List<ProductDTO> Products { get; set; } = [];

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
