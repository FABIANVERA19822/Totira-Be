namespace Totira.Support.CommonLibrary.Configurations
{
    public class SesSettings
    {
        public string AwsEmailAccessKey { get; set; } = string.Empty;
        public string AwsEmailAccessSecretKey { get; set; } = string.Empty;
        public string AwsSourceEmail { get; set; } = string.Empty;
        public bool SendEmails { get; set; } = false;
    }
}