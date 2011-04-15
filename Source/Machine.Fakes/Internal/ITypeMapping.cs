using System;

namespace Machine.Fakes.Internal
{
    interface ITypeMapping
    {
        Type InterfaceType { get; }

        void Configure(IContainer container);
    }
}