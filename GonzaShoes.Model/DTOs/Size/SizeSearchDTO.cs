namespace GonzaShoes.Model.DTOs.Size
{
    public class SizeSearchDTO
    {
        public decimal? Number { get; set; }
        public ActivationStateEnum? ActivationState { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
