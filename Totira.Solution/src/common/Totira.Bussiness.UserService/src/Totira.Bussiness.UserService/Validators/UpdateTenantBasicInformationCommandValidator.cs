using MongoDB.Driver;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{

    public class UpdateTenantBasicInformationCommandValidator : IMessageValidator<UpdateTenantBasicInformationCommand>
    {
        public ValidationResult Validate(UpdateTenantBasicInformationCommand command)
        {
            List<string> errors = new List<string>();

            //FirstName
            if (string.IsNullOrWhiteSpace(command.FirstName))
                errors.Add("FirstName is required.");

            //LastName
            if (string.IsNullOrWhiteSpace(command.LastName))
                errors.Add("LastName is required.");

            //Birthday
            if ((command.Birthday.Year == 0) || (command.Birthday.Month == 0) || (command.Birthday.Day == 0))
                errors.Add("Birthday is required.");



            return new ValidationResult(errors);
        }
    }
}
