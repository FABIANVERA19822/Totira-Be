using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Updated
{
    public class UpdateClaimJiraTicketCreationValidator : IMessageValidator<UpdateClaimJiraTicketCreationCommand>
    {
        public ValidationResult Validate(UpdateClaimJiraTicketCreationCommand command)
        {
            var errors = new List<string>();

            if (command.ClaimId == Guid.Empty)
                errors.Add("ClaimId cannot be empty.");

            return new ValidationResult(errors);
        }
    }
}
