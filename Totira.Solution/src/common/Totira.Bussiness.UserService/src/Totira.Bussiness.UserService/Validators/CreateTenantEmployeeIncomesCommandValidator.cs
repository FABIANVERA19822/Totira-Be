using System.Net.Mime;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantEmployeeIncomesCommandValidator : IMessageValidator<CreateTenantEmployeeIncomesCommand>
    {
        public ValidationResult Validate(CreateTenantEmployeeIncomesCommand command)
        {
            const int maxFileSize = 20 * 1024 * 1024; //20MB
            var errors = new List<string>();
            var allowedTypes = new List<string>()
            {
                MediaTypeNames.Image.Jpeg,
                "image/png",
                MediaTypeNames.Application.Pdf
            };

            //Company
            if (string.IsNullOrWhiteSpace(command.CompanyOrganizationName))
                errors.Add("Company or Organization Name is required.");
            if (command.CompanyOrganizationName != command.CompanyOrganizationName.TrimStart().TrimEnd())
                errors.Add("White space at the beginning or the end is not allowed.");
            if (command.CompanyOrganizationName.Contains("\x0020\x0020"))
                errors.Add("Two consecutive white spaces are not allowed.");
            if (command.CompanyOrganizationName.Length < 1 || command.CompanyOrganizationName.Length > 255)
                errors.Add("Company or Organization Name should have between 1 and 255 characters.");

            //Position
            if (string.IsNullOrWhiteSpace(command.Position))
                errors.Add("Position is required.");
            if (command.Position != command.Position.TrimStart().TrimEnd())
                errors.Add("White space at the beginning or the end is not allowed.");
            if (command.Position.Contains("\x0020\x0020"))
                errors.Add("Two consecutive white spaces are not allowed.");
            if (command.Position.Length < 1 || command.Position.Length > 255)
                errors.Add("Position should have between 1 and 255 characters.");

            //Files
            if (command.Files.Any(x => !allowedTypes.Contains(x.ContentType)))
                errors.Add("Invalid format file. Please upload a supported format file.");
            if (command.Files.Any(x => x.Size > maxFileSize))
                errors.Add("The file exceeds the supported size. Please try again with a smaller file.");

            return new ValidationResult(errors);
        }

        private static bool IsValidStartDate(DateTime startDate)
        {
            DateTime maxDate = DateTime.Today.AddYears(1);
            DateTime minDate = DateTime.Today.AddYears(-50);

            var valid = startDate.Year <= maxDate.Year && startDate.Year >= minDate.Year;

            return valid;
        }
    }
}
