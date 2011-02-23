using System;
using System.Linq.Expressions;
using Rhino.Mocks;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    public class RhinoFakeEngine : IFakeEngine
    {
        public object CreateFake(Type interfaceType)
        {
            var stub = MockRepository.GenerateStub(interfaceType);
            stub.Replay();
            return stub;
        }

        public T PartialMock<T>(params object[] args) where T : class
        {
            var mock = MockRepository.GenerateMock<T>(args);
            mock.Replay();
            return mock;
        }

        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TDependency, TReturnValue>(
            TDependency fake,
            Expression<Func<TDependency, TReturnValue>> func) where TDependency : class
        {
            var compiledFunction = func.Compile();

            return new RhinoQueryOptions<TReturnValue>(fake.Stub(f => compiledFunction(f)));
        }

        public ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            var compiledFunction = func.Compile();

            return new RhinoCommandOptions(fake.Stub(compiledFunction));
        }

        public void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            var compiledFunction = func.Compile();

            fake.AssertWasNotCalled(compiledFunction);
        }

        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class 
        {
            var compiledFunction = func.Compile();

            return new RhinoMethodCallOccurance<TFake>(fake, compiledFunction);
        }
    }
}