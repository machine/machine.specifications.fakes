using System;

namespace Machine.Fakes.Internal
{
    interface IMapping
    {
        Type InterfaceType { get; }

        object Resolve(Func<Type, object> instantiateType);
    }
}