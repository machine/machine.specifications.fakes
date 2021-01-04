using System;
using System.Collections.Generic;
using Machine.Fakes.ReSharperAnnotations;
using Machine.Specifications.Fakes.Sdk;

namespace Machine.Specifications.Fakes
{
    /// <summary>
    /// Base class for specifications.
    /// </summary>
    /// <typeparam name="TSubject">The subject of the specification</typeparam>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public abstract class WithFakes<TSubject, TFakeEngine>
        where TSubject : class
        where TFakeEngine : IFakeEngine, new()
    {
        /// <summary>
        /// The specification controller
        /// </summary>
        protected static SpecificationController<TSubject, TFakeEngine> _specificationController;

        /// <summary>
        /// Creates a new instance of the <see cref="WithFakes{TSubject, TFakeEngine}"/> class.
        /// </summary>
        protected WithFakes()
        {
            _specificationController = new SpecificationController<TSubject, TFakeEngine>();
        }

        /// <summary>
        ///   Creates a fake of the type specified by <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">The type to create a fake for. (Should be an interface or an abstract class)</typeparam>
        /// <param name="args">
        ///  Optional constructor parameters for abstract base classes as fakes.
        /// </param>
        /// <returns>
        ///   An newly created fake implementing <typeparamref name = "TInterfaceType" />.
        /// </returns>
        public static TInterfaceType An<TInterfaceType>(params object[] args) where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.An<TInterfaceType>(args);
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public static IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.Some<TInterfaceType>();
        }

        /// <summary>
        /// Creates a list of fakes.
        /// </summary>
        /// <typeparam name="TInterfaceType">
        /// Specifies the item type of the list. This should be an interface or an abstract class.
        /// </typeparam>
        /// <param name="amount">
        /// Specifies the amount of fakes that have to be created and inserted into the list.
        /// </param>
        /// <returns>
        /// An <see cref="IList{TInterfaceType}"/>.
        /// </returns>
        public static IList<TInterfaceType> Some<TInterfaceType>(int amount) where TInterfaceType : class
        {
            GuardAgainstStaticContext();

            return _specificationController.Some<TInterfaceType>(amount);
        }

        /// <summary>
        /// Configures the specification to execute a behavior config before the action on the subject
        /// is executed (<see cref="Because"/>).
        /// </summary>
        /// <typeparam name="TBehaviorConfig">Specifies the type of the config to be executed.</typeparam>
        /// <returns>The behavior config instance.</returns>
        /// <remarks>
        /// The class specified by <typeparamref name="TBehaviorConfig"/>
        /// needs to have private fields assigned with either <see cref="OnEstablish"/>
        /// or <see cref="OnCleanup"/> delegates.
        /// </remarks>
        protected static TBehaviorConfig With<TBehaviorConfig>() where TBehaviorConfig : new()
        {
            GuardAgainstStaticContext();

            return _specificationController.With<TBehaviorConfig>();
        }

        /// <summary>
        /// Configures the specification to execute the behavior config specified
        /// by <paramref name = "behaviorConfig" /> before the action on the sut is executed (<see cref = "Because" />).
        /// </summary>
        /// <param name = "behaviorConfig">
        /// Specifies the behavior config to be executed.
        /// </param>
        /// <remarks>
        /// The object specified by <see paramref="behaviorConfig"/>
        /// needs to have private fields assigned with either <see cref="OnEstablish"/>
        /// or <see cref="OnCleanup"/> delegates.
        /// </remarks>
        protected static void With(object behaviorConfig)
        {
            GuardAgainstStaticContext();

            _specificationController.With(behaviorConfig);
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

        /// <summary>
        /// Throws when the constructor has not been called
        /// </summary>
        protected static void GuardAgainstStaticContext()
        {
            if (_specificationController == null)
                throw new InvalidOperationException(
                    "WithFakes has not been initialized yet. Are you calling it from a static initializer?");
        }

        Cleanup after = () =>
        {
            _specificationController.Dispose();
            _specificationController = null;
        };
    }

    /// <summary>
    /// Base class for the simpler cases than <see cref="WithSubject{TSubject, TFakeEngine}"/>. 
    /// </summary>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    public abstract class WithFakes<TFakeEngine> : WithFakes<object, TFakeEngine> where TFakeEngine : IFakeEngine, new()
    {
    }
}
