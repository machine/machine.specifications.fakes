using System;

namespace Machine.Specifications.Fakes.Internal
{
    class ObjectMapping : IMapping
    {
        readonly object implementation;

        public Type InterfaceType { get; }

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
