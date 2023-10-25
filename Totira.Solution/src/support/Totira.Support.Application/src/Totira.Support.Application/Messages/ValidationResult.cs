namespace Totira.Support.Application.Messages
{
    public class ValidationResult
    {

        public ValidationResult(IEnumerable<string> errors)
        {
            Errors = errors ?? new List<string>();
        }


        public bool IsValid => Errors.Count() == 0;


        public IEnumerable<string> Errors { get; }
    }
}
