using System.Collections.Generic;
using Machine.Fakes.Sdk;
using Machine.Specifications;

namespace Machine.Fakes
{
    /// <summary>
    /// Base class that adds auto mocking (grasp), I mean auto faking capabilities
    /// to Machine.Specifications. 
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject for the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    /// <typeparam name="TFakeEngine">
    /// Specifies the concrete fake engine that will be used for creating fake instances.
    /// This must be a class with a parameterless constructor that implements <see cref="IFakeEngine"/>.
    /// </typeparam>
    public abstract class WithSubject<TSubject, TFakeEngine> 
        where TSubject : class
        where TFakeEngine : IFakeEngine, new()
    {
        protected static SpecificationController<TSubject, TFakeEngine> _specificationController;
        
        /// <summary>
        /// Creates a new instance of the <see cref="WithSubject{TSubject, TFakeEngine}"/> class.
        /// </summary>
        protected WithSubject()
        {
            _specificationController = new SpecificationController<TSubject, TFakeEngine>();
        }

        /// <summary>
        /// Gives access to the subject under specification. On first access
        /// the spec tries to create an instance of the subject type by itself.
        /// Override this behavior by manually setting a subject instance.
        /// </summary>
        protected static TSubject Subject
        {
            get { return _specificationController.Subject; }
            set { _specificationController.Subject = value; }
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
        public static TInterfaceType The<TInterfaceType>() where TInterfaceType : class
        {
            return _specificationController.The<TInterfaceType>();
        }

        /// <summary>
        ///   Creates a fake of the type specified by <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">The type to create a fake for. (Should be an interface or an abstract class)</typeparam>
        /// <returns>
        ///   An newly created fake implementing <typeparamref name = "TInterfaceType" />.
        /// </returns>
        public static TInterfaceType An<TInterfaceType>() where TInterfaceType : class
        {
            return _specificationController.An<TInterfaceType>();
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public static IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
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
            return _specificationController.Some<TInterfaceType>(amount);
        }

        /// <summary>
        ///   Uses the instance supplied by <paramref name = "instance" /> during the
        ///   creation of the sut. The specified instance will be injected into the constructor.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the interface type.</typeparam>
        /// <param name = "instance">Specifies the instance to be used for the specification.</param>
        public static void Use<TInterfaceType>(TInterfaceType instance) where TInterfaceType : class
        {
            _specificationController.Use(instance);
        }

        /// <summary>
        ///   Configures the specification to execute the <see cref = "IBehaviorConfig" /> specified
        ///   by <typeparamref name = "TBehaviorConfig" /> before the action on the sut is executed (<see cref = "Because" />).
        /// </summary>
        /// <typeparam name = "TBehaviorConfig">
        ///   Specifies the type of the config to be executed.
        /// </typeparam>
        protected static TBehaviorConfig With<TBehaviorConfig>() where TBehaviorConfig : IBehaviorConfig, new()
        {
            return _specificationController.With<TBehaviorConfig>();
        }

        /// <summary>
        ///   Configures the specification to execute the <see cref = "IBehaviorConfig" /> specified
        ///   by <paramref name = "behaviorConfig" /> before the action on the sut is executed (<see cref = "Because" />).
        /// </summary>
        /// <param name = "behaviorConfig">
        ///   Specifies the behavior config to be executed.
        /// </param>
        protected static void With(IBehaviorConfig behaviorConfig)
        {
            _specificationController.With(behaviorConfig);
        }

        Cleanup after = () => _specificationController.Dispose();
    }
}