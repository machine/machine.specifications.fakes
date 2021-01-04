using System;
using System.Linq.Expressions;
using Machine.Specifications.Fakes.Sdk;

namespace Machine.Specifications.Fakes.Internal
{
    class FakeEngineGateway
    {
        private static IFakeEngine _fakeEngine;

        public static void EngineIs(IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            _fakeEngine = fakeEngine;
        }

        public static IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.SetUpQueryBehaviorFor(fake, func);
        }

        public static ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.SetUpCommandBehaviorFor(fake, func);
        }

        public static void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            _fakeEngine.VerifyBehaviorWasNotExecuted(fake, func);
        }

        public static IMethodCallOccurrence VerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.VerifyBehaviorWasExecuted(fake, func);
        }

        public static T Fake<T>(params object[] args)
        {
            return _fakeEngine.Stub<T>(args);
        }
    }
}
