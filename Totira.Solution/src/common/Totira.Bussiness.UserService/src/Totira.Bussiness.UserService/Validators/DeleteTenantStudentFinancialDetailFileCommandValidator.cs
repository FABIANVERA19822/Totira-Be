using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class DeleteTenantStudentFinancialDetailFileCommandValidator : IMessageValidator<DeleteTenantStudentFinancialDetailFileCommand>
{
    public ValidationResult Validate(DeleteTenantStudentFinancialDetailFileCommand message)
    {
        var errors = new List<string>();

        if (string.IsNullOrWhiteSpace(message.FileName))
            errors.Add("File name is required.");

        if (message.TenantId == Guid.Empty)
            errors.Add("Tenant id is required.");

        return new ValidationResult(errors);
    }
}
