﻿using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class DeleteTenantApplicationRequestCommandValidator : IMessageValidator<DeleteTenantApplicationRequestCommand>
    {
        public ValidationResult Validate(DeleteTenantApplicationRequestCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
