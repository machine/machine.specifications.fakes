using System;

namespace Machine.Fakes.Internal
{
    class TypeMapping : IMapping
    {
        readonly Type implementationType;

        public Type InterfaceType { get; private set; }

        public TypeMapping(Type interfaceType, Type implementationType)
        {
            InterfaceType = interfaceType;
            this.implementationType = implementationType;
        }

        public object Resolve(Func<Type, object> instantiateType)
        {
            return instantiateType(implementationType);
        }
    }
}