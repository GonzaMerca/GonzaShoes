namespace GonzaShoes.Model.DTOs.Order
{
    public class OrderSearchDTO
    {
        public DateTime? DateFrom { get; set; }
        public DateTime? DateTo { get; set; }
        public int? OrderId { get; set; }
        public int? PaymentMethodId { get; set; }
        public int? UserId { get; set; }
        public ActivationStateEnum? ActivationState { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
