
using System.Text.RegularExpressions;
using Totira.Bussiness.UserService.Commands;
using Totira.Bussiness.UserService.Enums;
using Totira.Support.Application.Messages;

namespace Totira.Bussiness.UserService.Validators
{
    public class UpdateGroupApplicationCommandValidator : IMessageValidator<UpdateGroupApplicationCommand>
    {
        public ValidationResult Validate(UpdateGroupApplicationCommand command)
        {
            List<string> errors = new List<string>();
            int coApplicantCounter = 0;
            int guarantorCounter = 0;

            if (command.GroupApplicationProfiles.Count == 4)
            {
                foreach (var item in command.GroupApplicationProfiles)
                {
                    if (!Regex.IsMatch(item.Email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase))
                    {
                        errors.Add("Email is not in correct format");
                    }
                    if (item.InvinteeType == (int)InvinteeTypes.CoApplicant)
                    {
                        coApplicantCounter++;
                    }
                    else
                    {
                        guarantorCounter++;

                    }
                }
                if (coApplicantCounter > 3 || guarantorCounter > 1)
                    errors.Add("The tenant will be able to add up to 3 co-applicant and 1 guarantor only ");
            }

            if (command.GroupApplicationProfiles.Count > 4)
            {
                errors.Add("The allowed number of  invitations is 4  ");
            }

            return new ValidationResult(errors);
        }
    }
}
