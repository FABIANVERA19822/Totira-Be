using System;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
	public class CreateTenantContactLandlordCommandValidator : IMessageValidator<CreateTenantContactLandlordCommand>
    {
        public ValidationResult Validate(CreateTenantContactLandlordCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

