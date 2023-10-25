namespace Totira.Bussiness.UserService.DTO
{
    public class ValidateOtpDtoAnt
    {
        public Guid EntityId { get; set; } = Guid.Empty;

        public Guid? EntityKey { get; set; }

        public string Scope { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public ValidateOtpDtoAnt()
        {
            EntityId = Guid.Empty;
            Scope = string.Empty;
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public ValidateOtpDtoAnt(Guid entityId, Guid? entityKey, string scope)
        {
            this.EntityId = entityId;
            this.EntityKey = entityKey;
            this.Scope = scope;
            this.IsSuccess = true;
        }

        public ValidateOtpDtoAnt(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public ValidateOtpDtoAnt(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }
    }
}