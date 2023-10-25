using Totira.Support.Application.Messages;

namespace Totira.Support.Application.Exception
{
    public class ValidationException : System.Exception
    {
        public ValidationException(IMessage validatedMessage, IEnumerable<string> errors) : base()
        {
            ValidatedMessage = validatedMessage;
            Errors = errors;
        }

        /// <summary>
        /// The command validated.
        /// </summary>
        public IMessage ValidatedMessage { get; }

        /// <summary>
        /// The list of validation errors.
        /// </summary>
        public IEnumerable<string> Errors { get; }
    }
}
