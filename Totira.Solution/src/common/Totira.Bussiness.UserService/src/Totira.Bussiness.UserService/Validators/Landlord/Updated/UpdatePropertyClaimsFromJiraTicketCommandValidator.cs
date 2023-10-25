namespace Totira.Bussiness.UserService.Validators.Landlord.Updated
{
    using System.Collections.Generic;
    using Totira.Bussiness.UserService.Commands.LandlordCommands;
    using Totira.Support.Application.Messages;

    internal class UpdatePropertyClaimsFromJiraTicketCommandValidator : IMessageValidator<UpdatePropertyClaimsFromJiraTicketCommand>
    {
        public ValidationResult Validate(UpdatePropertyClaimsFromJiraTicketCommand message)
        {
            var errors = new List<string>();
            return new ValidationResult(errors);
        }
    }
}