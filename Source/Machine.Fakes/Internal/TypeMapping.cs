using System;

namespace Machine.Fakes.Internal
{
    class TypeMapping : IMapping
    {
        public TypeMapping(Type interfaceType, Type implementationType)
        {
            InterfaceType = interfaceType;
            ImplementationType = implementationType;
        }

        public Type ImplementationType { get; private set; }
        public Type InterfaceType { get; private set; }

        public void Configure(IContainer container)
        {
            container.Register(this);
        }
    }
}