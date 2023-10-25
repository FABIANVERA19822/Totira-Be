namespace Totira.Bussiness.UserService.Handlers.Queries
{
    using Totira.Bussiness.UserService.Queries;
    using Totira.Support.Application.Queries;
    using Totira.Support.Otp;
    using Totira.Support.Otp.DTO;

    public class ValidateOtpQueryHandler : IQueryHandler<QueryValidateOtp, ValidateOtpDto>
    {
        private readonly IOtpHandler _otpHandler;

        public ValidateOtpQueryHandler(IOtpHandler otpHandler)
        {
            _otpHandler = otpHandler;
        }

        public async Task<ValidateOtpDto> HandleAsync(QueryValidateOtp query)
        {
            return await _otpHandler.ValidateOtpAsync(query.OtpId, query.AccessCode);
        }
    }
}