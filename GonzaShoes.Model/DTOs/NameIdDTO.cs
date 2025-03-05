namespace GonzaShoes.Model.DTOs
{
    public class NameIdDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Tag { get; set; } = string.Empty;
        public List<NameIdDTO> NameIds { get; set; } = [];
        public string PicturePath { get; set; } = string.Empty;
    }
}
