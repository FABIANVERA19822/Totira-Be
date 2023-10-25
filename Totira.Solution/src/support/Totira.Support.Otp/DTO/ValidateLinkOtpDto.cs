namespace Totira.Support.Otp.DTO
{
    public class ValidateLinkOtpDto
    {
        public Guid OtpId { get; set; } = Guid.Empty;

        public string Email { get; set; } = string.Empty;

        public string AccessToken { get; set; } = string.Empty;

        public bool IsSuccess { get; set; } = false;

        public string ErrorMessage { get; set; } = string.Empty;

        public ValidateLinkOtpDto()
        {
            OtpId = Guid.Empty;
            Email = string.Empty;
            AccessToken = string.Empty;
            IsSuccess = false;
            ErrorMessage = string.Empty;
        }

        public ValidateLinkOtpDto(Guid otpId, string email, string accessToken)
        {
            this.OtpId = otpId;
            this.Email = email;
            this.AccessToken = accessToken;
            this.IsSuccess = true;
        }

        public ValidateLinkOtpDto(string errorMessage)
        {
            this.ErrorMessage = errorMessage;
        }

        public ValidateLinkOtpDto(bool isSuccess, string errorMessage)
        {
            this.IsSuccess = isSuccess;
            this.ErrorMessage = errorMessage;
        }
    }
}