using System;
using System.Collections.Generic;
using Machine.Fakes.Utils;

namespace Machine.Fakes.Internal
{
    class SpecificationController<TSubject, TFakeEngine> : SpecificationController<TSubject> 
        where TSubject : class 
        where TFakeEngine : IFakeEngine, new()
    {
        public SpecificationController() : base(new TFakeEngine())
        {
        }
    }

    class SpecificationController<TSubject> : IFakeAccessor, IDisposable where TSubject : class
    {
        private readonly List<IBehaviorConfig> _behaviorConfigs = new List<IBehaviorConfig>();
        private TSubject _specificationSubject;
        private readonly AutoFakeContainer<TSubject> _container;
        
        public SpecificationController(IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            _container = new AutoFakeContainer<TSubject>(fakeEngine);
            FakeEngineGateway.EngineIs(_container);
        }

        public TSubject Subject
        {
            get { return _specificationSubject ?? (_specificationSubject = _container.CreateSubject()); }
            set { _specificationSubject = value; }
        }

        public void Use<TInterfaceType>(TInterfaceType instance) where TInterfaceType : class
        {
            _container.Inject(typeof (TInterfaceType), instance);
        }

        public TBehaviorConfig With<TBehaviorConfig>() where TBehaviorConfig : IBehaviorConfig, new()
        {
            var behaviorConfig = new TBehaviorConfig();
            With(behaviorConfig);
            return behaviorConfig;
        }

        public void With(IBehaviorConfig behaviorConfig)
        {
            Guard.AgainstArgumentNull(behaviorConfig, "behaviorConfig");

            _behaviorConfigs.Add(behaviorConfig);

            behaviorConfig.EstablishContext(this);
        }

        public TInterfaceType An<TInterfaceType>() where TInterfaceType : class
        {
            return (TInterfaceType)_container.CreateFake(typeof(TInterfaceType));
        }

        public TInterfaceType The<TInterfaceType>() where TInterfaceType : class
        {
            return _container.Get<TInterfaceType>();
        }

        public IList<TInterfaceType> Some<TInterfaceType>() where TInterfaceType : class
        {
            return _container.CreateFakeCollectionOf<TInterfaceType>();
        }

        public void Dispose()
        {
            _behaviorConfigs.ForEach(x => x.CleanUp(_specificationSubject));
            _behaviorConfigs.Clear();
        }
    }
}