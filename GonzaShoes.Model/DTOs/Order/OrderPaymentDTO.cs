namespace GonzaShoes.Model.DTOs.Order
{
    public class OrderPaymentDTO : ContextualProps
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public decimal PayWith { get; set; }
        public decimal Cash { get; set; }
        public decimal DebitCard { get; set; }
        public decimal CreditCard { get; set; }
        public decimal Transfer { get; set; }
    }
}
