namespace GonzaShoes.Model.DTOs.Product
{
    public class ProductSearchDTO
    {
        public string Name { get; set; } = string.Empty;
        public List<int> BrandIds { get; set; } = [];
        public List<int> ModelProductIds { get; set; } = [];
        public List<int> ColorIds { get; set; } = [];
        public List<int> SizeIds { get; set; } = [];
        public ActivationStateEnum? ActivationState { get; set; }
        public bool OnlyWithStock { get; set; }

        public bool GetActivationState()
        {
            return this.ActivationState == ActivationStateEnum.Active ? true : false;
        }
    }
}
