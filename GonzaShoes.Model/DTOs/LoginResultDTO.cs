namespace GonzaShoes.Model.DTOs
{
    public class LoginResultDTO
    {
        public ValidationResultDTO ValidationResult { get; set; } = new ValidationResultDTO();
        public string Token { get; set; } = string.Empty;
    }
}
