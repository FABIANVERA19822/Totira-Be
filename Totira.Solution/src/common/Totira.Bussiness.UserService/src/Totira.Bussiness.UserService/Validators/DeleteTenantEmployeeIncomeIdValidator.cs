
namespace Totira.Bussiness.UserService.Validators
{
    using Totira.Bussiness.UserService.Commands;
    using Totira.Support.Application.Messages;

    public class DeleteTenantEmployeeIncomeIdValidator : IMessageValidator<DeleteTenantEmployeeIncomeIdCommand>
    {
        public ValidationResult Validate(DeleteTenantEmployeeIncomeIdCommand message)
        {
            var errors = new List<string>(); 
            return new ValidationResult(errors);
        }
    }
}
