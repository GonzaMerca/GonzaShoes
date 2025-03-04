namespace GonzaShoes.Model.DTOs.Color
{
    public class ColorDTO : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

    }
}
