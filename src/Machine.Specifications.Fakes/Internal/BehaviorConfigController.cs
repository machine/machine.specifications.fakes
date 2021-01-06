using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications.Fakes.Sdk;

namespace Machine.Specifications.Fakes.Internal
{
    class BehaviorConfigController
    {
        readonly List<object> _behaviorConfigs = new List<object>();

        public void Establish(object behaviorConfig, IFakeAccessor fakeAccessor)
        {
            if (behaviorConfig == null)
            {
                throw new ArgumentNullException(nameof(behaviorConfig));
            }

            if (fakeAccessor == null)
            {
                throw new ArgumentNullException(nameof(fakeAccessor));
            }

            behaviorConfig
                .GetFieldValues<OnEstablish>()
                .Each(establishDelegate => establishDelegate(fakeAccessor));

            _behaviorConfigs.Add(behaviorConfig);
        }

        public void CleanUp(object subject)
        {
            if (subject == null)
            {
                throw new ArgumentNullException(nameof(subject));
            }

            _behaviorConfigs
                .SelectMany(config => config.GetFieldValues<OnCleanup>())
                .Each(cleanupDelegate => cleanupDelegate(subject));

            _behaviorConfigs.ForEach(config => config.ResetReferences());
            _behaviorConfigs.Clear();
        }
    }
}
