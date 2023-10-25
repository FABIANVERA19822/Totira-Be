using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class UpdateTenantRentalHistoriesReactivateCommandValidator : IMessageValidator<UpdateTenantRentalHistoriesReactivateCommand>
    {
        public ValidationResult Validate(UpdateTenantRentalHistoriesReactivateCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
