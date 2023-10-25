using MongoDB.Driver;
using System.Text.RegularExpressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantContactInformationCommandValidator : IMessageValidator<CreateTenantContactInformationCommand>
    {
        public ValidationResult Validate(CreateTenantContactInformationCommand command)
        {
            List<string> errors = new List<string>();

            //HousingStatus
            if (string.IsNullOrWhiteSpace(command.HousingStatus))
                errors.Add("HousingStatus is required.");

            //SelectedCountry
            if (string.IsNullOrWhiteSpace(command.SelectedCountry))
                errors.Add("Country is required.");

            //Province
            if (string.IsNullOrWhiteSpace(command.Province))
                errors.Add("Province is required.");
            if (command.Province.Length < 1 || command.Province.Length > 70)
                errors.Add("Province should have between 1 and 70 characters.");

            //City
            if (string.IsNullOrWhiteSpace(command.City))
                errors.Add("City is required.");
            if (command.City.Length < 1 || command.City.Length > 70)
                errors.Add("City should have between 1 and 70 characters.");

            //ZipCode
            Regex zipCodCharacters = new Regex(@"^[A-Za-z0-9]+$");

            if (string.IsNullOrWhiteSpace(command.ZipCode))
                errors.Add("Postal Code is required.");
            if (command.ZipCode.Length < 1 || command.ZipCode.Length > 6)
                errors.Add("Postal Code should have between 1 and 6 characters.");
            if (!Regex.IsMatch(command.ZipCode, @"^[A-Za-z0-9]+$"))
                errors.Add("Postal Code should be alphanumeric.");

            //StreetAddress
            if (string.IsNullOrWhiteSpace(command.StreetAddress))
                errors.Add("StreetAddress is required.");
            if (command.StreetAddress.Length < 1 || command.StreetAddress.Length > 100)
                errors.Add("StreetAddress should have between 1 and 100 characters.");

            //Unit
            if (!string.IsNullOrWhiteSpace(command.Unit)) {
                if (command.Unit.Length < 1 || command.Unit.Length > 70)
                    errors.Add("unit should have between 1 and 70 characters.");
            }
            

            return new ValidationResult(errors);
        }
    }
}
