namespace GonzaShoes.Model.Shoe
{
    public class Product : ContextualProps
    {
        public int Id { get; set; }
        
        // Relationship with Model
        public int ModelId { get; set; }
        public Model Model { get; set; } = null!;

        // Relationship with Color
        public int ColorId { get; set; }
        public Color Color { get; set; } = null!;

        // Relationship with Size
        public int SizeId { get; set; }
        public Size Size { get; set; } = null!;

        public decimal Price { get; set; }
        public int Stock { get; set; }
    }
}
