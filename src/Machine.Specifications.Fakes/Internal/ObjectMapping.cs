using System;

namespace Machine.Specifications.Fakes.Internal
{
    class ObjectMapping : IMapping
    {
        readonly object implementation;

        public Type InterfaceType { get; private set; }

        public ObjectMapping(Type interfaceType, object implementation)
        {
            InterfaceType = interfaceType;
            this.implementation = implementation;
        }

        public object Resolve(Func<Type, object> instantiateType)
        {
            return implementation;
        }
    }
}
