using System;
using System.Collections.Concurrent;
using Machine.Fakes.Internal;
using Machine.Fakes.Sdk;
using Machine.Specifications.Utility;

namespace Machine.Fakes
{
    public class Registrar
    {
        readonly ConcurrentDictionary<Type, ITypeMapping> _mappings = new ConcurrentDictionary<Type, ITypeMapping>();

        internal void Store(ITypeMapping mapping)
        {
            Guard.AgainstArgumentNull(mapping, "mapping");

            _mappings.GetOrAdd(mapping.InterfaceType, mapping);
        }

        internal void Configure(IContainer container)
        {
            _mappings.Values.Each(m => m.Configure(container));
        }

        public static Registrar New(Action<Registrar> configurationExpression)
        {
            var registrar = new Registrar();
            configurationExpression(registrar);
            return registrar;
        }

        public RegistrationExpression For(Type type)
        {
            return new RegistrationExpression(type, this);
        }

        public RegistrationExpression<T> For<T>()
        {
            return new RegistrationExpression<T>(this);
        }

        public sealed class RegistrationExpression
        {
            readonly Type _interfaceType;
            readonly Registrar _register;

            internal RegistrationExpression(Type interfaceType, Registrar register)
            {
                _interfaceType = interfaceType;
                _register = register;
            }

            public void Use(object implementation)
            {
                _register.Store(new ObjectMapping(_interfaceType, implementation));
            }

            public void Use(Type implementationType)
            {
                _register.Store(new TypeMapping(_interfaceType, implementationType));
            }
        }

        public sealed class RegistrationExpression<T>
        {
            readonly Registrar _register;

            internal RegistrationExpression(Registrar register)
            {
                _register = register;
            }

            public void Use<TOther>() where TOther : T
            {
                _register.Store(new TypeMapping(typeof (T), typeof (TOther)));
            }

            public void Use(T instance)
            {
                _register.Store(new ObjectMapping(typeof (T), instance));
            }

            public void Use(Func<T> factory)
            {
                _register.Store(new FactoryMapping<T>(factory));
            }
        }
    }
}