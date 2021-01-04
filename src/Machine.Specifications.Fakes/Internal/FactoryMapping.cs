using System;

namespace Machine.Specifications.Fakes.Internal
{
    class FactoryMapping<TInterface> : IMapping
    {
        readonly Func<TInterface> factory;

        public FactoryMapping(Func<TInterface> factory)
        {
            this.factory = factory;
        }

        public Type InterfaceType
        {
            get { return typeof(TInterface); }
        }

        public object Resolve(Func<Type, object> instantiateType)
        {
            return factory();
        }
    }
}
