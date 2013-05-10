using System;

using Machine.Fakes.Sdk;
using Machine.Specifications;
using Machine.Specifications.Factories;

namespace Machine.Fakes
{
    /// <summary>
    /// Base class that adds auto mocking (grasp), I mean auto faking capabilities
    /// to Machine.Specifications. 
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject of the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    public abstract class WithSubject<TSubject, TFakeEngine> : WithFakes<TSubject, TFakeEngine>
        where TSubject : class
        where TFakeEngine : IFakeEngine, new()
    {
        /// <summary>
        /// Creates a new instance of the <see cref="WithSubject{TSubject, TFakeEngine}"/> class.
        /// </summary>
        protected WithSubject()
        {
            _specificationController = new SpecificationController<TSubject, TFakeEngine>();

            ContextFactory.ChangeAllowedNumberOfBecauseBlocksTo(2);
        }

        /// <summary>
        /// Gives access to the subject under specification. On first access
        /// the spec tries to create an instance of the subject type by itself.
        /// Override this behavior by manually setting a subject instance.
        /// </summary>
        protected static TSubject Subject
        {
            get
            {
                GuardAgainstStaticContext();

                return _specificationController.Subject;
            }

            set
            {
                GuardAgainstStaticContext();
                
                _specificationController.Subject = value;
            }
        }

        /// <summary>
        ///   Creates a fake of the type specified by <typeparamref name = "TInterfaceType" />.
        ///   This method reuses existing instances. If an instance of <typeparamref name = "TInterfaceType" />
        ///   was already requested it's returned here. (You can say this is kind of a singleton behavior)
        ///   Besides that, you can obtain a reference to automatically injected fakes with this
        ///   method.
        /// </summary>
        /// <typeparam name = "TInterfaceType">The type to create a fake for. (Should be an interface or an abstract class)</typeparam>
        /// <returns>
        ///   An instance implementing <typeparamref name="TInterfaceType" />.
        /// </returns>
        protected static TInterfaceType The<TInterfaceType>() where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.The<TInterfaceType>();
        }

        /// <summary>
        ///     Uses the instance supplied by <paramref name = "instance" /> during the
        ///     build process of the subject. The specified instance will be injected into the constructor.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the interface type.</typeparam>
        /// <param name = "instance">Specifies the instance to be used for the specification.</param>
        protected static void Configure<TInterfaceType>(TInterfaceType instance)
        {
            GuardAgainstStaticContext();
            
            _specificationController.Configure(instance);
        }

        /// <summary>
        /// Registered the type specified via <typeparamref name="TImplementationType"/> as the default type
        /// for the interface specified via <typeparamref name="TInterfaceType"/>. With this the type gets automatically
        /// build when the subject is resolved.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// Specifies the interface type.
        /// </typeparam>
        /// <typeparam name="TImplementationType">
        /// Specifies the implementation type.
        /// </typeparam>
        protected static void Configure<TInterfaceType, TImplementationType>() where TImplementationType : TInterfaceType
        {
            GuardAgainstStaticContext();

            _specificationController.Configure<TInterfaceType, TImplementationType>();
        }

        /// <summary>
        /// Applies the configuration embedded in the registar to the underlying container.
        /// </summary>
        /// <param name="registrar">
        /// Specifies the registrar.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the supplied registrar is <c>null</c>.
        /// </exception>
        protected static void Configure(Registrar registrar)
        {
            Guard.AgainstArgumentNull(registrar, "registar");
            GuardAgainstStaticContext();

            _specificationController.Configure(registrar);
        }

        /// <summary>
        /// Applies the configuration embedded in the registar to the underlying container.
        /// Shortcut for <see cref="Configure(Registrar)"/> so that you don't have to create the 
        /// registrar manually.
        /// </summary>
        /// <typeparam name="TRegistrar">The registrar type.</typeparam>
        /// <returns>The registrar.</returns>
        protected static TRegistrar Configure<TRegistrar>() where TRegistrar : Registrar, new()
        {
            GuardAgainstStaticContext();

            var registrar = new TRegistrar();
            _specificationController.Configure(registrar);
            return registrar;
        }

        /// <summary>
        /// Shortcut for <see cref="Configure(Registrar)"/>. This one will create
        /// a registrar for you and allow configuration via the delegate passed
        /// in via <paramref name="registrarExpression"/>.
        /// </summary>
        /// <param name="registrarExpression">
        /// Specifies the configuration for the registrar.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when the supplied registrar is <c>null</c>.
        /// </exception>
        protected static void Configure(Action<Registrar> registrarExpression)
        {
            Guard.AgainstArgumentNull(registrarExpression, "registar");
            GuardAgainstStaticContext();

            _specificationController.Configure(registrarExpression);
        }

        Because of = () => _specificationController.EnsureSubjectCreated();

        Cleanup after = () => ContextFactory.ChangeAllowedNumberOfBecauseBlocksTo(1);
    }
}