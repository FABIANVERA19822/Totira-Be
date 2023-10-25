using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryValidateOtp : IQuery
    {
        public Guid OtpId { get; set; }

        public string AccessCode { get; set; }

        public QueryValidateOtp(Guid otpId, string accessCode)
        {
            this.OtpId = otpId;
            this.AccessCode = accessCode;
        }
    }
}