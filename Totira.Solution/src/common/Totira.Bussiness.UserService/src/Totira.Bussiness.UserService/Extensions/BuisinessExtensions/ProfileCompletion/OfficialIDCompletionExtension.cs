
using Totira.Bussiness.UserService.DTO.ThirdpartyService;

namespace Totira.Bussiness.UserService.Extensions.BuisinessExtensions.ProfileCompletion
{
    public static class OfficialIDCompletionExtension
    {
        public static bool OfficialIDComplete(this GetPersonaApplicationDto personaValidation)
        {
            return personaValidation != null &&
                   !string.IsNullOrWhiteSpace(personaValidation.Status) &&
                   (personaValidation.Status.ToLower() == "approved" || personaValidation.Status.ToLower() == "completed");
        }
    }
}
