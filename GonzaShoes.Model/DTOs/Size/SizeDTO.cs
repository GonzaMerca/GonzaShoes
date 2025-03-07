namespace GonzaShoes.Model.DTOs.Size
{
    public class SizeDTO : ContextualProps, ICloneable
    {
        public int Id { get; set; }
        public decimal Number { get; set; }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
