using System;

namespace Machine.Specifications.Fakes.Internal
{
    class TypeMapping : IMapping
    {
        readonly Type implementationType;

        public Type InterfaceType { get; }

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
