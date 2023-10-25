using Totira.Support.Otp.DTO;

namespace Totira.Support.Otp
{
    public interface IOtpHandler
    {
        Task<ValidateLinkOtpDto> ValidateLinkOtpAsync(Guid otpId);
        Task<ValidateOtpDto> ValidateOtpAsync(Guid otpId, string accessCode);
        Task SetOtpProcessAsync(
           Guid otpId,
           Guid entityId,
           Guid? entityKey,
           string email,
           string accessCode,
           double expiration,
           string scope,
           bool encryptAccessCode = true,
           bool oneOtp = false);

        Task SetOtpProcessAsync(
           Guid otpId,
           Guid entityId,
           Guid? entityKey,
           string email,
           string accessCode,
           string scope,
           bool encryptAccessCode = true,
           bool oneOtp = false);

        Task SetOtpProcessAsync(
           Guid otpId,
           Guid entityId,
           string email,
           string accessCode,
           double expiration,
           string scope,
           bool encryptAccessCode = true,
           bool oneOtp = false);

        Task SetOtpProcessAsync(
           Guid otpId,
           Guid entityId,
           string email,
           string accessCode,
           string scope,
           bool encryptAccessCode = true,
           bool oneOtp = false);

        Task UpdateOtpProcessAsync(Guid entityKey, string email, bool isActive, double expiration);
    }
}