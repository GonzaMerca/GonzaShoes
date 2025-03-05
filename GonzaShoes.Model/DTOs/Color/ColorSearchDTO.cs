namespace GonzaShoes.Model.DTOs.Color
{
    public class ColorSearchDTO
    {
        public string Name { get; set; } = string.Empty;
        public string HexCode { get; set; } = string.Empty;
        public ActivationStateEnum? ActivationState { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
