using Microsoft.Extensions.DependencyInjection;
using Totira.Support.Application.Dispatchers.Behaviours;

namespace Totira.Support.Application.Dispatchers
{
    public class DispatcherFactory
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Type> _behaviourTypes = new List<Type>();

        public DispatcherFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        public DispatcherFactory AddBehaviour<TBehaviour>() where TBehaviour : IBehaviour
        {
            _behaviourTypes.Add(typeof(TBehaviour));

            return this;
        }


        public IDispatcher Create()
        {
            var behaviours = _behaviourTypes.Select(type => (IBehaviour)ActivatorUtilities.CreateInstance(_serviceProvider, type));

            return new MessageDispatcher(_serviceProvider, behaviours);
        }
    }
}
