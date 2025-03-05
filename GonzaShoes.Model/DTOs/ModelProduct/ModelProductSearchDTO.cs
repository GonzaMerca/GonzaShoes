namespace GonzaShoes.Model.DTOs.ModelProduct
{
    public class ModelProductSearchDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<int> BrandIds { get; set; } = [];
        public ActivationStateEnum? ActivationState { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
