using System.Collections.Generic;
using System.Linq;
using Machine.Fakes.Sdk;
using Machine.Specifications.Runner.Impl;

namespace Machine.Fakes.Internal
{
    class BehaviorConfigController
    {
        readonly List<object> _behaviorConfigs = new List<object>();

        public void Establish(object behaviorConfig, IFakeAccessor fakeAccessor)
        {
            Guard.AgainstArgumentNull(behaviorConfig, "behaviorConfig");
            Guard.AgainstArgumentNull(fakeAccessor, "fakeAccessor");

            _behaviorConfigs.Add(behaviorConfig);

            _behaviorConfigs
                .SelectMany(config => config.GetFieldValues<OnEstablish>())
                .ForEach(establishDelegate => establishDelegate(fakeAccessor));
        }

        public void CleanUp(object subject)
        {
            Guard.AgainstArgumentNull(subject, "subject");

            _behaviorConfigs
                .SelectMany(config => config.GetFieldValues<OnCleanUp>())
                .ForEach(cleanupDelegate => cleanupDelegate(subject));
        }
    }
}