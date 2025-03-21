﻿using System.ComponentModel.DataAnnotations.Schema;

namespace GonzaShoes.Model.Entities.Order
{
    [Table("OrderPayment")]
    public class OrderPayment : ContextualProps
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
        public decimal Amount { get; set; }
        public decimal PayWith { get; set; }
        public decimal Cash { get; set; }
        public decimal DebitCard { get; set; }
        public decimal CreditCard { get; set; }
        public decimal Transfer { get; set; }
    }
}
