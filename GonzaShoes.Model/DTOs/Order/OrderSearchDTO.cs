namespace GonzaShoes.Model.DTOs.Order
{
    public class OrderSearchDTO
    {
        public string Name { get; set; } = string.Empty;
        public ActivationStateEnum? ActivationState { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
