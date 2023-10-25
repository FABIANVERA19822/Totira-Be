namespace Totira.Support.CommonLibrary.Interfaces
{
    public interface IEmailHandler
    {
        Task<bool> SendEmailAsync(string email, string subject, string emailBody);
        Task<bool> SendBulkEmailAsync(List<string> listEmails, string subject, string emailBody);

    }
}
