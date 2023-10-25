using Newtonsoft.Json.Linq;

namespace Totira.Support.CommonLibrary.Interfaces
{
    public interface IMaskObject
    {
        List<T> ModifyPropertiesListObjectToAsterisk<T>(List<T> objs, Type maskType);
        T ModifyPropertiesToAsterisk<T>(T obj, Type maskType);
        void ModifyPropertiesToAsteriskJT(JToken token);
    }
}