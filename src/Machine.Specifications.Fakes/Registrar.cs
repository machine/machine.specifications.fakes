using System;
using System.Collections.Concurrent;
using Machine.Specifications.Fakes.Internal;
using Machine.Specifications.Fakes.Sdk;

namespace Machine.Specifications.Fakes
{
    /// <summary>
    /// Registration endpoint for configuring the underlying container with 
    /// concrete implementations (the rest that can be faked will be filled up from the AutoFakeContainer).
    /// </summary>
    public class Registrar
    {
        readonly ConcurrentDictionary<Type, IMapping> _mappings = new ConcurrentDictionary<Type, IMapping>();

        /// <summary>
        /// Stores the mapping supplied by <paramref name="mapping"/>.
        /// </summary>
        /// <param name="mapping">
        /// Specifies the mapping to be stored.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="mapping"/> is <c>null</c>.
        /// </exception>
        void Store(IMapping mapping)
        {
            _mappings.GetOrAdd(mapping.InterfaceType, mapping);
        }

        /// <summary>
        /// Applies the configured mappings.
        /// </summary>
        /// <param name="register">An action that registers a single mapping</param>
        internal void Apply(Action<IMapping> register)
        {
            _mappings.Values.Each(register);
        }

        /// <summary>
        /// Shortcut for inlining the configuration of a registrar via
        /// the nested closure pattern.
        /// </summary>
        /// <param name="configurationExpression">
        /// Specifies the expression that does the configuraition.
        /// </param>
        /// <returns>
        /// The configured registrar.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="configurationExpression"/> is <c>null</c>.
        /// </exception>
        public static Registrar New(Action<Registrar> configurationExpression)
        {
            if (configurationExpression == null)
            {
                throw new ArgumentNullException(nameof(configurationExpression));
            }

            var registrar = new Registrar();
            configurationExpression(registrar);
            return registrar;
        }

        /// <summary>
        /// Starts the configuration of a type. This should be the interface type.
        /// </summary>
        /// <param name="type">
        /// Specifies the interface type.
        /// </param>
        /// <returns>
        /// An expression for further configuration.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="type"/> is <c>null</c>.
        /// </exception>
        public RegistrationExpression For(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return new RegistrationExpression(type, this);
        }

        /// <summary>
        /// Starts the configuration of a type. This should be the interface type.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the interface type.
        /// </typeparam>
        /// <returns>
        /// An expression for further configuration.
        /// </returns>
        public RegistrationExpression<T> For<T>()
        {
            return new RegistrationExpression<T>(this);
        }

        /// <summary>
        /// Concludes the configurataion that was startet with <see cref="For(Type)"/>.
        /// </summary>
        public sealed class RegistrationExpression
        {
            readonly Type _interfaceType;
            readonly Registrar _registrar;

            internal RegistrationExpression(Type interfaceType, Registrar registrar)
            {
                _interfaceType = interfaceType;
                _registrar = registrar;
            }

            /// <summary>
            /// Configures the underlying container to use
            /// the specified instance whenever an instance of the interface type
            /// in the container is resolved.
            /// </summary>
            /// <param name="implementation">
            /// Specifies the instance to be used.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// Thrown when <paramref name="implementation"/> is <c>null</c>.
            /// </exception>
            public void Use(object implementation)
            {
                if (implementation == null)
                {
                    throw new ArgumentNullException(nameof(implementation));
                }

                _registrar.Store(new ObjectMapping(_interfaceType, implementation));
            }

            /// <summary>
            /// Configures the underlying container to use
            /// the specified implementation type whenever an instance of the interface type
            /// in the container is resolved.
            /// </summary>
            /// <param name="implementationType">
            /// Specifies the implementation type to be used.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// Thrown when <paramref name="implementationType"/> is <c>null</c>.
            /// </exception>
            public void Use(Type implementationType)
            {
                if (implementationType == null)
                {
                    throw new ArgumentNullException(nameof(implementationType));
                }

                _registrar.Store(new TypeMapping(_interfaceType, implementationType));
            }
        }

        /// <summary>
        /// Concludes the configuration that was started with <see cref="For(Type)"/>.
        /// </summary>
        public sealed class RegistrationExpression<T>
        {
            readonly Registrar _registrar;

            internal RegistrationExpression(Registrar registrar)
            {
                _registrar = registrar;
            }

            /// <summary>
            /// Configures the underlying container to use
            /// the specified implementation type whenever an instance of the interface type
            /// in the container is resolved.
            /// </summary>
            /// <typeparam name="TImplementationType">
            /// Specifies the implementation type.
            /// </typeparam>
            public void Use<TImplementationType>() where TImplementationType : T
            {
                _registrar.Store(new TypeMapping(typeof(T), typeof(TImplementationType)));
            }

            /// <summary>
            /// Configures the underlying container to use
            /// the specified instance whenever an instance of the interface type
            /// in the container is resolved.
            /// </summary>
            /// <param name="instance">
            /// Specifies the instance to be used.
            /// </param>
            public void Use(T instance)
            {
                _registrar.Store(new ObjectMapping(typeof(T), instance));
            }

            /// <summary>
            /// Configures the underlying container to use
            /// the specified factory when he needs to create an instance of the target type.
            /// </summary>
            /// <param name="factory">
            /// Specifies the factory to be used for creation.
            /// </param>
            /// <exception cref="ArgumentNullException">
            /// Thrown when <paramref name="factory"/> is <c>null</c>.
            /// </exception>
            public void Use(Func<T> factory)
            {
                if (factory == null)
                {
                    throw new ArgumentNullException(nameof(factory));
                }

                _registrar.Store(new FactoryMapping<T>(factory));
            }
        }
    }
}
