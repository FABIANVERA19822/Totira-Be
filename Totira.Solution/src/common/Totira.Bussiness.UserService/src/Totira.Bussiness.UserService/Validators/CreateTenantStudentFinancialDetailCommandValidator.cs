using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class CreateTenantStudentFinancialDetailCommandValidator : IMessageValidator<CreateTenantStudentFinancialDetailCommand>
{
    public ValidationResult Validate(CreateTenantStudentFinancialDetailCommand message)
    {
        var errors = new List<string>();

        return new ValidationResult(errors);
    }
}
