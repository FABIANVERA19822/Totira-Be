using System;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class DeleteTenantCoSignerFromGroupApplicationProfileCommandValidator : IMessageValidator<DeleteTenantCoSignerFromGroupApplicationProfileCommand>
    {
        public ValidationResult Validate(DeleteTenantCoSignerFromGroupApplicationProfileCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}

