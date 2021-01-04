using System;

namespace Machine.Specifications.Fakes.Internal
{
    interface IMapping
    {
        Type InterfaceType { get; }

        object Resolve(Func<Type, object> instantiateType);
    }
}
