using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.User
{
    [Table("User")]
    public class User : ContextualProps
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
