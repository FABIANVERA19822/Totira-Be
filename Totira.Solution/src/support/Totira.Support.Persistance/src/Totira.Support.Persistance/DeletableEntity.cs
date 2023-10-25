using Totira.Support.Persistance.Entities;

namespace Totira.Support.Persistance
{
    public class DeletableEntity : Entity, IDeletable
    {
        public bool Deleted { get; set; }
    }
}
