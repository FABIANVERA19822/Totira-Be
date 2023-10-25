using System;
using System.Text.RegularExpressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class CreateTenantShareProfileCommandValidator : IMessageValidator<CreateTenantShareProfileCommand>
    {
        public ValidationResult Validate(CreateTenantShareProfileCommand command)
        {
            List<string> errors = new List<string>();


            if (!Regex.IsMatch(command.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                errors.Add("Email is not in correct format");


            if (command.Message.Length > 255)
                errors.Add("Message should have less than 255 characters.");

            return new ValidationResult(errors);
        }
    }
}

