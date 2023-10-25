using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators;

public class UpdateTenantStudentFinancialDetailCommandValidator : IMessageValidator<UpdateTenantStudentFinancialDetailCommand>
{
    public ValidationResult Validate(UpdateTenantStudentFinancialDetailCommand message)
    {
        var errors = new List<string>();
        
        return new ValidationResult(errors);
    }
}
