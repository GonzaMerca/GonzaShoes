namespace GonzaShoes.Model
{
    public class ContextualProps
    {
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public DateTime? DateUpdated { get; set; }
        public int CreatedUserId { get; set; }
        public int? UpdatedUserId { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
