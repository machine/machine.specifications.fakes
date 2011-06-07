using System;
using Machine.Fakes.Sdk;

namespace Machine.Fakes
{
    /// <summary>
    /// This class implements the several way that can be used to work with the
    /// <see cref="Registrar"/> API.
    /// </summary>
    public static class FakeAccessorRegistrarExtensions
    {
        /// <summary>
        ///     Uses the instance supplied by <paramref name = "instance" /> during the
        ///     creation of the sut. The specified instance will be injected into the constructor.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the interface type.</typeparam>
        /// <param name = "instance">Specifies the instance to be used for the specification.</param>
        /// <param name = "accessor">
        ///     Specifies the fake accessor
        /// </param>
        public static void Configure<TInterfaceType>(
            this IFakeAccessor accessor,
            TInterfaceType instance)
        {
            Guard.AgainstArgumentNull(accessor, "accessor");

            accessor.Configure(Registrar.New(config => config.For<TInterfaceType>().Use(instance)));
        }

        /// <summary>
        ///     Registered the type specified via <typeparamref name = "TImplementationType" /> as the default type
        ///     for the interface specified via <typeparamref name = "TInterfaceType" />. With this the type gets automatically
        ///     build when the subject is resolved.
        /// </summary>
        /// <param name = "accessor">
        ///     Specifies the fake accessor
        /// </param>
        /// <typeparam name = "TInterfaceType">
        ///     Specifies the interface type.
        /// </typeparam>
        /// <typeparam name = "TImplementationType">
        ///     Specifies the implementation type.
        /// </typeparam>
        public static void Configure<TInterfaceType, TImplementationType>(
            this IFakeAccessor accessor)
            where TImplementationType : TInterfaceType
        {
            Guard.AgainstArgumentNull(accessor, "accessor");

            accessor.Configure(Registrar.New(config => config.For<TInterfaceType>().Use<TImplementationType>()));
        }

        /// <summary>
        ///     Shortcut for <see cref = "IFakeAccessor.Configure" />. This one will create
        ///     a registrar for you and allow configuration via the delegate passed
        ///     in via <paramref name = "registrarExpression" />.
        /// </summary>
        /// <param name = "accessor">
        ///     Specifies the fake accessor
        /// </param>
        /// <param name = "registrarExpression">
        ///     Specifies the configuration for the registrar.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        ///     Thrown when the supplied registrar expression is <c>null</c>.
        /// </exception>
        public static void Configure(
            this IFakeAccessor accessor,
            Action<Registrar> registrarExpression)
        {
            Guard.AgainstArgumentNull(accessor, "accessor");
            Guard.AgainstArgumentNull(registrarExpression, "registrarExpression");

            accessor.Configure(Registrar.New(registrarExpression));
        }
    }
}