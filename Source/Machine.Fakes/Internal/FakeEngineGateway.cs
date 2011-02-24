using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;

namespace Machine.Fakes.Internal
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

        public static IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            return _fakeEngine.VerifyBehaviorWasExecuted(fake, func);
        }

        public static T Fake<T>()
        {
            return _fakeEngine.Stub<T>();
        }
    }
}