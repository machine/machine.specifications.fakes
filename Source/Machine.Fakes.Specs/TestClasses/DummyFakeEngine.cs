using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;

namespace Machine.Fakes.Specs.TestClasses
{
    public class DummyFakeEngine : IFakeEngine
    {
        public object CreatedFake { get; set; }
        public Type RequestedFakeType { get; private set; }

        public object CreateFake(Type interfaceType)
        {
            RequestedFakeType = interfaceType;
            return CreatedFake;
        }

        public T PartialMock<T>(params object[] args) where T : class
        {
            throw new NotImplementedException();
        }

        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(TFake fake, Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            throw new NotImplementedException();
        }

        public ICommandOptions SetUpCommandBehaviorFor<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            throw new NotImplementedException();
        }

        public void VerifyBehaviorWasNotExecuted<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            throw new NotImplementedException();
        }

        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            throw new NotImplementedException();
        }

        public IMatcher<TReturnValue> CreateMatcher<TReturnValue>()
        {
            throw new NotImplementedException();
        }
    }
}