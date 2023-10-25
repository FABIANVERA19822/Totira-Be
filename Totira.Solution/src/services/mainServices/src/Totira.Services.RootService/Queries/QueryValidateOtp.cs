namespace Totira.Services.RootService.Queries
{
    public class QueryValidateOtp
    {
        public Guid OtpId { get; set; }
        public string AccessCode { get; set; } = string.Empty;
    }
}
