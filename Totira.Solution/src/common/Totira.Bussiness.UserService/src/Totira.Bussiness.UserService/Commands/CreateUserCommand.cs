using Totira.Support.Application.Commands;
using Totira.Support.EventServiceBus.Attributes;

namespace Totira.Bussiness.UserService.Commands
{
    [RoutingKey("CreateUserCommand")]
    public class CreateUserCommand : ICommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }

        public CreateUserCommand(Guid id, string name, string lastName)
        {
            Id = id;
            Name = name;
            LastName = lastName;
        }
    }
}
