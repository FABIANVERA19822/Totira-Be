using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Totira.Bussiness.UserService.Commands.LandlordCommands.Create;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Created
{
    internal class CreateLandlordPropertyDisplayValidator : IMessageValidator<CreateLandlordPropertyDisplayCommand>
    {
        public ValidationResult Validate(CreateLandlordPropertyDisplayCommand command)
        {
            List<string> errors = new List<string>();

            //FirstName
            if (string.IsNullOrWhiteSpace(command.MLSId))
                errors.Add("MLSId is required.");

            //LastName
            if (command.MLSId.Length!=8)
                errors.Add("MLDId must be eight charaters long.");


            return new ValidationResult(errors);
        }
    }
}
