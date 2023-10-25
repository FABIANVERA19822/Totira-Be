using Totira.Support.Application.Queries;

namespace Totira.Bussiness.UserService.Queries
{
    public class QueryValidateLinkOtp : IQuery
    {
        public Guid OtpId { get; set; }

        public QueryValidateLinkOtp(Guid otpId)
        {
            this.OtpId = otpId;
        }
    }
}