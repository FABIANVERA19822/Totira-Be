using System.Net.Mime;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Created
{
    public class CreatedLandlordIdentityValidator : IMessageValidator<CreateLandlordIdentityCommand>
    {
        public ValidationResult Validate(CreateLandlordIdentityCommand command)
        {
            List<string> errors = new List<string>();

            const int maxFileSize = 20 * 1024 * 1024; //20MB
            var allowedTypes = new List<string>()
            {
                MediaTypeNames.Image.Jpeg,
                "image/png",
                MediaTypeNames.Application.Pdf
            };

            //LandlordId
            if (command.LandlordId == Guid.Empty)
                errors.Add("LandlordId is required.");

            //Phone Number
            if (string.IsNullOrEmpty(command.PhoneNumber.Number) || string.IsNullOrEmpty(command.PhoneNumber.CountryCode))
            {
                errors.Add("Phone number is required.");
            }

            //Identity Proof Files
            if (command.IdentityProofs.Any(x => !allowedTypes.Contains(x.ContentType)))
                errors.Add("Invalid format file. Please upload a supported format file.");
            if (command.IdentityProofs.Any(x => x.Size > maxFileSize))
                errors.Add("The file exceeds the supported size. Please try again with a smaller file.");
            if (command.IdentityProofs.Count() > 3)
                errors.Add("Only up to three identity proof files can be uploaded.");

            return new ValidationResult(errors);
        }
    }
}
