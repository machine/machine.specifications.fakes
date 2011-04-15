using System;

namespace Machine.Fakes.Internal
{
    class FactoryMapping<TInterface> : IMapping
    {
        public FactoryMapping(Func<TInterface> factory)
        {
            Factory = factory;
        }

        public Func<TInterface> Factory { get; private set; }

        public Type InterfaceType
        {
            get { return typeof (TInterface); }
        }

        public void Configure(IContainer container)
        {
            container.Register(this);
        }
    }
}