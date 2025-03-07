using GonzaShoes.Model.DTOs.ModelProduct;

namespace GonzaShoes.Model.DTOs.Brand
{
    public class BrandDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // A brand has many models
        public List<ModelProductDTO> Models { get; set; } = [];

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
