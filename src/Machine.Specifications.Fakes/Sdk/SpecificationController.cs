using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications.Fakes.Internal;

namespace Machine.Specifications.Fakes.Sdk
{
    /// <summary>
    /// Shortcut for <see cref="SpecificationController{TSubject}"/> which
    /// supplies the type of the fake engine to be used via a generic type parameter.
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject for the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    /// <typeparam name="TFakeEngine">
    /// Specifies the type of the fake engine which will be used.
    /// </typeparam>
    public class SpecificationController<TSubject, TFakeEngine> : SpecificationController<TSubject>
        where TSubject : class
        where TFakeEngine : IFakeEngine, new()
    {
        /// <summary>
        /// Creates a new instance of the <see cref="SpecificationController{TSubject, TFakeEngine}"/> class.
        /// </summary>
        public SpecificationController() : base(new TFakeEngine())
        {
        }
    }

    /// <summary>
    /// Controller that implements all the core capabilities of Machine.Fakes.
    /// This includes filling a subject with fakes and providing all the handy helper methods
    /// for interacting with fakes in a specification.
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject for the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    public class SpecificationController<TSubject> : IFakeAccessor, IDisposable where TSubject : class
    {
        private readonly BehaviorConfigController _behaviorConfigController = new BehaviorConfigController();
        private readonly AutoFakeContainer _container;
        private TSubject _specificationSubject;

        /// <summary>
        /// Creates a new instance of the <see cref="SpecificationController{TSubject}"/> class.
        /// </summary>
        /// <param name="fakeEngine">
        /// Specifies the <see cref="IFakeEngine"/> that is used for creating specifications.
        /// </param>
        public SpecificationController(IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            _container = new AutoFakeContainer(fakeEngine);

            FakeEngineGateway.EngineIs(fakeEngine);
        }

        /// <summary>
        /// Gives access to the subject under specification. On first access
        /// the spec tries to create an instance of the subject type by itself.
        /// Override this behavior by manually setting a subject instance.
        /// </summary>
        public TSubject Subject
        {
            get { return _specificationSubject ?? (_specificationSubject = _container.CreateSubject<TSubject>()); }
            set { _specificationSubject = value; }
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
        public void Configure(Registrar registrar)
        {
            Guard.AgainstArgumentNull(registrar, "registar");

            registrar.Apply(_container.Register);
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
        public TBehaviorConfig With<TBehaviorConfig>() where TBehaviorConfig : new ()
        {
            var behaviorConfig = new TBehaviorConfig();
            With(behaviorConfig);
            return behaviorConfig;
        }

        /// <summary>
        ///   Configures the specification to execute the behavior config specified
        ///   by <paramref name = "behaviorConfig" /> before the action on the sut is executed (<see cref = "Because" />).
        /// </summary>
        /// <param name = "behaviorConfig">
        ///   Specifies the behavior config to be executed.
        /// </param>
        /// <remarks>
        /// The object specified by <see paramref="behaviorConfig"/>
        /// needs to have private fields assigned with either <see cref="OnEstablish"/>
        /// or <see cref="OnCleanup"/> delegates.
        /// </remarks>
        public void With(object behaviorConfig)
        {
            _behaviorConfigController.Establish(behaviorConfig, this);
        }

        /// <summary>
        ///   Creates a fake of the type specified by <typeparamref name = "TInterfaceType" /> without a default constructor.
        /// </summary>
        /// <typeparam name = "TInterfaceType">The type to create a fake for.</typeparam>
        /// <param name="args">
        ///   No default constructor arguments
        /// </param>
        /// <returns>
        ///   An newly created fake implementing <typeparamref name = "TInterfaceType" />.
        /// </returns>
        public TInterfaceType An<TInterfaceType>(params object[] args) where TInterfaceType : class
        {
            return (TInterfaceType)An(typeof(TInterfaceType), args);
        }

        /// <summary>
        /// Creates a fake of the type specified by <paramref name="interfaceType"/> without default constructor.
        /// </summary>
        /// The type to create a fake for.
        /// <param name="interfaceType">
        /// Specifies the type of item to fake.
        /// </param>
        /// <param name="args">
        /// Specifies the constructor parameters if the class is a concrete class without default constructor
        /// </param>
        /// <returns>
        /// An newly created fake implementing <paramref name="interfaceType"/>.
        /// </returns>
        public object An(Type interfaceType, params object[] args)
        {
            return _container.CreateFake(interfaceType, args);
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
        public TInterfaceType The<TInterfaceType>() where TInterfaceType : class
        {
            return _container.Get<TInterfaceType>();
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            return Some<TInterfaceType>(3);
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
        public IList<TInterfaceType> Some<TInterfaceType>(int amount) where TInterfaceType : class
        {
            if (amount < 0)
                throw new ArgumentOutOfRangeException("amount");

            return Enumerable.Range(0, amount)
                .Select(x => (TInterfaceType)_container.CreateFake(typeof(TInterfaceType)))
                .ToList();
        }

        /// <summary>
        /// Performs cleanup. Exuecutes the Cleanup functionality of all configured behavior configs.
        /// </summary>
        public void Dispose()
        {
            _behaviorConfigController.CleanUp(Subject);
        }

        /// <summary>
        /// Ensures that the subject has been created. This will trigger the lazy loading in case creation hasn't happened
        /// before.
        /// </summary>
        public void EnsureSubjectCreated()
        {
            // ReSharper disable ReturnValueOfPureMethodIsNotUsed
            Subject.ToString();
            // ReSharper restore ReturnValueOfPureMethodIsNotUsed
        }
    }
}
