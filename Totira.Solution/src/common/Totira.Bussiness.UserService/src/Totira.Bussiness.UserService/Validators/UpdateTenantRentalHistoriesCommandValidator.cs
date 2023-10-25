using System.Text.RegularExpressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class UpdateTenantRentalHistoriesCommandValidator : IMessageValidator<UpdateTenantRentalHistoriesCommand>
    {

        public ValidationResult Validate(UpdateTenantRentalHistoriesCommand command)
        {
            List<string> errors = new List<string>();


            if (!Regex.IsMatch(command.ContactInformation!.EmailAddress, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                errors.Add("Email is not in correct format");


            return new ValidationResult(errors);
        }
    }
}

