namespace GonzaShoes.Model.DTOs.Color
{
    public class ColorDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
