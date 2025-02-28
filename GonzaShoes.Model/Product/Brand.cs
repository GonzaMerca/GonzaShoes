namespace GonzaShoes.Model.Shoe
{
    public class Brand : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;

        // A brand has many models
        public List<Model> Models { get; set; } = [];
    }
}
