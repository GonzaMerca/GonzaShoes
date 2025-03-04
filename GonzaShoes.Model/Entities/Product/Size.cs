using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Product
{
    [Table("Size")]
    public class Size : ContextualProps
    {
        public int Id { get; set; }
        public decimal Number { get; set; }
    }
}
