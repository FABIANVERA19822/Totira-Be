using System.Reflection;

namespace Totira.Support.EventServiceBus.Attributes
{
    public static class CustomAttributeExtensions
    {
        /// <summary>
        /// Obtain the RoutingKey for the <T>.
        /// </summary>
        /// <typeparam name="T">The type of the IMessage.</typeparam>
        /// <returns> The RoutingKey value for the IMessage type.</returns>
        public static string GetRoutingKey<T>()
        {
            var dnAttribute = typeof(T).GetCustomAttribute<RoutingKeyAttribute>(true);
            if (dnAttribute != null)
            {
                return dnAttribute.RoutingKey;
            }
            return null;
        }

        /// <summary>
        /// Obtain the RoutingKey for the Type.
        /// </summary>
        /// <returns> The RoutingKey value for the IMessage type.</returns>
        public static string GetRoutingKey(Type type)
        {
            var dnAttribute = type.GetCustomAttribute<RoutingKeyAttribute>(true);
            if (dnAttribute != null)
            {
                return dnAttribute.RoutingKey;
            }
            return null;
        }
    }
}
