namespace Totira.Services.RootService.DTO
{
    public class ValidateOtpDto
    {
        public Guid EntityId { get; set; } = Guid.Empty;

        public Guid? EntityKey { get; set; }

        public string Scope { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public ValidateOtpDto()
        {
            EntityId = Guid.Empty;
            Scope = string.Empty;
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public ValidateOtpDto(Guid entityId, Guid? entityKey, string scope)
        {
            this.EntityId = entityId;
            this.EntityKey = entityKey;
            this.Scope = scope;
            this.IsSuccess = true;
        }

        public ValidateOtpDto(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public ValidateOtpDto(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }
    }
}