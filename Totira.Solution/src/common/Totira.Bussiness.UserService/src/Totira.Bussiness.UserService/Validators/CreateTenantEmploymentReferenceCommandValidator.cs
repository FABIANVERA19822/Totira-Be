using System.Text.RegularExpressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantEmploymentReferenceCommandValidator : IMessageValidator<CreateTenantEmploymentReferenceCommand>
    {
        public ValidationResult Validate(CreateTenantEmploymentReferenceCommand command)
        {
            List<string> errors = new List<string>();

            //FirstName
            if (command.FirstName.Length > 70)
                errors.Add("The first name field must be up to 70 characters");
            //LastNAme
            if (command.LastName.Length > 70)
                errors.Add("The Last name field must be up to 70 characters");
            //JobTitle
            if (command.JobTitle.Length < 1 || command.JobTitle.Length > 70)
                errors.Add("The job title should be between 1 and 70 characters");
            //Relationship
            if (command.Relationship.Length < 1 || command.Relationship.Length > 70)
                errors.Add("The Relationship should be between 1 and 70 characters");

            //Email
            if (!Regex.IsMatch(command.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                errors.Add("Email is not in correct format");




            return new ValidationResult(errors);
        }
    }
}

