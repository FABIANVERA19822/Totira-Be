using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Created
{
    public class CreatedLandlordBasicInfoValidator : IMessageValidator<CreateLandlordBasicInfoCommand>
    {
        public ValidationResult Validate(CreateLandlordBasicInfoCommand command)
        {
            List<string> errors = new List<string>();

            //FirstName
            if (string.IsNullOrWhiteSpace(command.FirstName))
                errors.Add("FirstName is required.");

            //LastName
            if (string.IsNullOrWhiteSpace(command.LastName))
                errors.Add("LastName is required.");


            return new ValidationResult(errors);
        }
    }
}
