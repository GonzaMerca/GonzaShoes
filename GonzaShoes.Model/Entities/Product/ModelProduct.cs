using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Product
{
    [Table("ModelProduct")]
    public class ModelProduct : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // Relationship with Brand
        public int BrandId { get; set; }
        public Brand Brand { get; set; } = null!;

        // A model has many products
        public List<Product> Products { get; set; } = [];

    }
}
