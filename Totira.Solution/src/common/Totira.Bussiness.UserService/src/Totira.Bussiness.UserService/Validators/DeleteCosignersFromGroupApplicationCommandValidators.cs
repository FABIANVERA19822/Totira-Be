using System;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;


namespace Totira.Bussiness.UserService.Validators;

public class DeleteCosignersFromGroupApplicationCommandValidators
{
    public class DeleteCosignersFromGroupApplicationCommandValidator : IMessageValidator<DeleteCosignersFromGroupApplicationCommand>
    {
        public ValidationResult Validate(DeleteCosignersFromGroupApplicationCommand message)
        {
            List<string> errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}
