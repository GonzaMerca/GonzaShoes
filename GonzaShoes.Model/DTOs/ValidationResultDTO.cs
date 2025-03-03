namespace GonzaShoes.Model.DTOs
{
    public class ValidationResultDTO
    {
        public List<string> ErrorMessages { get; set; }
        public bool HasErrors => ErrorMessages != null && ErrorMessages.Any();
        public List<string> WarningsMessages { get; set; }
        public bool HasWarnings => WarningsMessages != null && WarningsMessages.Any();
        public List<string> ConfirmMessages { get; set; }
        public bool HasConfirm => ConfirmMessages != null && ConfirmMessages.Any();

        public ValidationResultDTO()
        {
            ErrorMessages = new List<string>();
            ConfirmMessages = new List<string>();
            WarningsMessages = new List<string>();
        }

        public string GetConfirmMessages()
        {
            return string.Join(". ", ConfirmMessages);
        }

        public string GetErrorMessages()
        {
            return string.Join(". ", ErrorMessages);
        }

        public string GetWarningsMessages()
        {
            return string.Join(". ", WarningsMessages);
        }

        public void Fill(ValidationResultDTO validationResult)
        {
            if (validationResult.ErrorMessages.Count > 0)
                ErrorMessages.AddRange(validationResult.ErrorMessages);

            if (validationResult.WarningsMessages.Count > 0)
                WarningsMessages.AddRange(validationResult.WarningsMessages);

            if (validationResult.ConfirmMessages.Count > 0)
                ConfirmMessages.AddRange(validationResult.ConfirmMessages);
        }
    }
}
