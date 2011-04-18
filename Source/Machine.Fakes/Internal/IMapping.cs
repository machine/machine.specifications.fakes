using System;

namespace Machine.Fakes.Internal
{
    interface IMapping
    {
        Type InterfaceType { get; }

        void Configure(IContainer container);
    }
}