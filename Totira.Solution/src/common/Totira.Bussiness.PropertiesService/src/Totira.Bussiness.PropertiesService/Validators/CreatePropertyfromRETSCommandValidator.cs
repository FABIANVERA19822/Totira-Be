
namespace Totira.Bussiness.PropertiesService.Validators
{
    using Totira.Bussiness.PropertiesService.Commands;
    using Totira.Support.Application.Messages;

    public class CreatePropertyfromRETSCommandValidator : IMessageValidator<CreatePropertyfromRETSCommand>
    {
        public ValidationResult Validate(CreatePropertyfromRETSCommand message)
        {
            List<string> errors = new List<string>();
            
            return new ValidationResult(errors);
        }
    }
}
