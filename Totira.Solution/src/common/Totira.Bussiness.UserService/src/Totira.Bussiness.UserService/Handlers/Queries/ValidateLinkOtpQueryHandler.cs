namespace Totira.Bussiness.UserService.Handlers.Queries
{
    using Totira.Bussiness.UserService.Queries;
    using Totira.Support.Application.Queries;
    using Totira.Support.Otp;
    using Totira.Support.Otp.DTO;

    public class ValidateLinkOtpQueryHandler : IQueryHandler<QueryValidateLinkOtp, ValidateLinkOtpDto>
    {
        private readonly IOtpHandler _otpHandler;

        public ValidateLinkOtpQueryHandler(
            IOtpHandler otpHandler)
        {
            _otpHandler = otpHandler;
        }

        public async Task<ValidateLinkOtpDto> HandleAsync(QueryValidateLinkOtp query)
        {
            return await _otpHandler.ValidateLinkOtpAsync(query.OtpId);
        }
    }
}