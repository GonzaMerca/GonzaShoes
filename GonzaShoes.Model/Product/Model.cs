namespace GonzaShoes.Model.Shoe
{
    public class Model : ContextualProps
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
