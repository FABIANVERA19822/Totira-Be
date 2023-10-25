using Microsoft.AspNetCore.Identity;
using System.Net.Mime;
using Totira.Bussiness.UserService.Commands.LandlordCommands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Created
{
    public class CreatePropertyClaimsFromLandlordValidator : IMessageValidator<CreatePropertyClaimsFromLandlordCommand>
    {
        public ValidationResult Validate(CreatePropertyClaimsFromLandlordCommand command)
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

            //Role
            if (string.IsNullOrWhiteSpace(command.Role))
                errors.Add("Role is required.");

            foreach (var claim in command.ClaimDetails)
            {
                if (string.IsNullOrEmpty(claim.ListingUrl) ||
                    (string.IsNullOrEmpty(claim.Address) && string.IsNullOrEmpty(claim.City)) ||
                    string.IsNullOrEmpty(claim.MlsID))
                {
                    errors.Add("Property identifier is required.");
                }

                if (claim.OwnershipProofs is null || claim.OwnershipProofs.Count() == 0)
                {
                    errors.Add("Claim does not have ownership proof files.");
                }

                if (claim.OwnershipProofs is not null && claim.OwnershipProofs.Count() > 8)
                {
                    errors.Add("A claim cannot have more than 8 ownership files.");
                }

                if (claim.OwnershipProofs is not null && claim.OwnershipProofs.Any(x => x.Size > maxFileSize))
                {
                    errors.Add($"Files cannot be bigger than {maxFileSize}.");
                }

                if (claim.OwnershipProofs is not null && claim.OwnershipProofs.Any(x => !allowedTypes.Contains(x.ContentType)))
                {
                    errors.Add($"Files extension is not accepted.");
                }

            }
            return new ValidationResult(errors);
        }
    }
}
