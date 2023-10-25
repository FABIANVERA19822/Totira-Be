using System.Linq.Expressions;
using System.Reflection;
using Totira.Support.Application.Commands;

namespace Totira.Support.Application.Extensions
{
    public static class CommandExtension
    {
        public static TCommand Bind<TCommand, TProperty>(this TCommand command, Expression<Func<TCommand, TProperty>> expression, object value) where TCommand : ICommand
        {
            var memberExpression = expression.Body as MemberExpression;
            if (memberExpression is null)
            {
                memberExpression = ((UnaryExpression)expression.Body).Operand as MemberExpression;
            }
            var propertyName = memberExpression.Member.Name.ToLowerInvariant() ?? string.Empty;
            var commandType = command.GetType();
            var field = commandType.GetFields(BindingFlags.Instance | BindingFlags.NonPublic).SingleOrDefault(x => x.Name.ToLowerInvariant().StartsWith($"<{propertyName}>"));
            if (field is null)
            {
                return command;
            }
            field.SetValue(command, value);
            return command;
        }
    }
}
