using System;

namespace Machine.Fakes.Internal
{
    class ObjectMapping : ITypeMapping
    {
        public ObjectMapping(Type interfaceType, object implementation)
        {
            InterfaceType = interfaceType;
            Implementation = implementation;
        }

        public object Implementation { get; private set; }

        public Type InterfaceType { get; private set; }

        public void Configure(IContainer container)
        {
            container.Register(this);
        }
    }
}