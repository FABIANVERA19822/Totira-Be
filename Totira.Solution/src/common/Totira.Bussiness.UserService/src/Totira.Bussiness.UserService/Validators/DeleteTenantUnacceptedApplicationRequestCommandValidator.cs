using System;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
	public class DeleteTenantUnacceptedApplicationRequestCommandValidator : IMessageValidator<DeleteTenantUnacceptedApplicationRequestCommand>
    {

        public ValidationResult Validate(DeleteTenantUnacceptedApplicationRequestCommand command)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

