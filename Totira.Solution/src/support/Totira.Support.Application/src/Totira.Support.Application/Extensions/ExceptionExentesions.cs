using Totira.Support.Application.Events;
using Totira.Support.Application.Exception;

namespace Totira.Support.Application.Extensions;

public static class ExceptionExentesions
{
    public static T CreateValidatedEventOf<T>(this System.Exception exception, string defaultMessage = "An error occurred processing your request") where T : BaseValidatedEvent, new()
    {
        return exception is ValidationException ? 
                        new T { Errors = ((ValidationException)exception).Errors.ToArray() }
                        : new T { Errors = new [] { defaultMessage } };
    }
}