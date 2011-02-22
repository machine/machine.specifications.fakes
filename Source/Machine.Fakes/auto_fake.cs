using System;
using System.Collections.Generic;
using Machine.Fakes.Internal;
using Machine.Fakes.Utils;
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
    public abstract class auto_fake<TSubject> : IFakeAccessor where TSubject : class
    {
        private static auto_fake<TSubject> ExecutingSpec;
        private readonly List<IBehaviorConfig> BehaviorConfigs = new List<IBehaviorConfig>();
        private TSubject SpecificationSubject;
        private readonly AutoFakeContainer<TSubject> Container;
        
        /// <summary>
        /// Creates a new instance of the <see cref="auto_fake{T}"/> class.
        /// </summary>
        protected auto_fake()
        {
            ExecutingSpec = this;
            Container = new AutoFakeContainer<TSubject>(GetType());
        }

        /// <summary>
        /// Gives access to the subject under specification. On first access
        /// the spec tries to create an instance of the subject type by itself.
        /// Override this behavior by manually setting a subject instance.
        /// </summary>
        protected static TSubject Subject
        {
            get { return ExecutingSpec.SpecificationSubject ?? (ExecutingSpec.SpecificationSubject = ExecutingSpec.Container.CreateTarget()); }
            set { ExecutingSpec.SpecificationSubject = value; }
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
            return ExecutingSpec.Container.Get<TInterfaceType>();
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
            return ExecutingSpec.Container.Stub<TInterfaceType>();
        }

        /// <summary>
        ///   Creates a list containing 3 fake instances of the type specified
        ///   via <typeparamref name = "TInterfaceType" />.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the item type of the list. This should be an interface or an abstract class.</typeparam>
        /// <returns>An <see cref = "IList{T}" />.</returns>
        public static IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            return ExecutingSpec.Container.CreateFakeCollectionOf<TInterfaceType>();
        }

        /// <summary>
        ///   Uses the instance supplied by <paramref name = "instance" /> during the
        ///   creation of the sut. The specified instance will be injected into the constructor.
        /// </summary>
        /// <typeparam name = "TInterfaceType">Specifies the interface type.</typeparam>
        /// <param name = "instance">Specifies the instance to be used for the specification.</param>
        public static void Use<TInterfaceType>(TInterfaceType instance) where TInterfaceType : class
        {
            ExecutingSpec.Container.Inject(typeof (TInterfaceType), instance);
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
            var behaviorConfig = new TBehaviorConfig();
            With(behaviorConfig);
            return behaviorConfig;
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
            Guard.AgainstArgumentNull(behaviorConfig, "behaviorConfig");

            ExecutingSpec.BehaviorConfigs.Add(behaviorConfig);

            behaviorConfig.EstablishContext(ExecutingSpec);
        }

        TInterfaceType IFakeAccessor.An<TInterfaceType>()
        {
            return An<TInterfaceType>();
        }

        TInterfaceType IFakeAccessor.The<TInterfaceType>()
        {
            return The<TInterfaceType>();
        }

        IList<TInterfaceType> IFakeAccessor.Some<TInterfaceType>()
        {
            return Some<TInterfaceType>();
        }

        void IFakeAccessor.Use<TInterfaceType>(TInterfaceType instance)
        {
            Use(instance);
        }


        Cleanup after = () =>
        {
            ExecutingSpec.BehaviorConfigs.ForEach(x => x.CleanUp(Subject));
            ExecutingSpec = null;
        };
    }
}