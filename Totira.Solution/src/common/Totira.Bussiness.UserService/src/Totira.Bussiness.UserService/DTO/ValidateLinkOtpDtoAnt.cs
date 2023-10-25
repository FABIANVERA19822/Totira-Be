namespace Totira.Bussiness.UserService.DTO
{
    public class ValidateLinkOtpDtoAnt
    {
        public Guid OtpId { get; set; } = Guid.Empty;

        public string Email { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public ValidateLinkOtpDtoAnt()
        {
            OtpId = Guid.Empty;
            Email = string.Empty;
            AccessToken = string.Empty;
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public ValidateLinkOtpDtoAnt(Guid otpId, string email, string accessToken)
        {
            this.OtpId = otpId;
            this.Email = email;
            this.AccessToken = accessToken;
            this.IsSuccess = true;
        }

        public ValidateLinkOtpDtoAnt(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public ValidateLinkOtpDtoAnt(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }
    }
}