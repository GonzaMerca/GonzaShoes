namespace GonzaShoes.Model.Entities.Product
{
    public class Brand : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // A brand has many models
        public List<ModelProduct> Models { get; set; } = [];
    }
}
