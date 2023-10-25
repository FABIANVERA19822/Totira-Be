namespace Totira.Support.Otp.Configurations
{
    public class OtpOptions
    {
        public int Retries { get; set; } = 3;
        public int ExpirationTime { get; set; } = 3600;
        public int AmountUse { get; set; } = 1;
    }
}