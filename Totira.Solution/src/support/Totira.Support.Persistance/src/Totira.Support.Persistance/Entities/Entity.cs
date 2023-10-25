namespace Totira.Support.Persistance.Entities
{

    public class Entity : IEntity, IIdentifiable<Guid>
    {
        public Guid Id { get; set; }

        
    }
}
