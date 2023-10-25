using Totira.Bussiness.UserService.Commands.LandlordCommands.Update;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators.Landlord.Updated
{
    public class UpdateClaimJiraTicketResultValidator : IMessageValidator<UpdateClaimJiraTicketResultCommand>
    {
        public ValidationResult Validate(UpdateClaimJiraTicketResultCommand command)
        {
            var errors = new List<string>();

            if (command.ClaimId == Guid.Empty)
                errors.Add("ClaimId cannot be empty.");

            return new ValidationResult(errors);
        }
    }
}
