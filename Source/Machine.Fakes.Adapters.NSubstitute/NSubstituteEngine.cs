using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    /// <summary>
    ///   An implementation of <see cref = "IFakeEngine" />
    ///   using the NSubstitute framework.
    /// </summary>
    public class NSubstituteEngine : IFakeEngine
    {
        #region IFakeEngine Members

        public object CreateFake(Type interfaceType)
        {
            return Substitute.For(new[] {interfaceType}, null);
        }

        public T PartialMock<T>(params object[] args) where T : class
        {
            return Substitute.For<T>(args);
        }

        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            return new NSubstituteQueryOptions<TFake, TReturnValue>(fake, func);
        }

        public ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            return new NSubstituteCommandOptions<TFake>(fake, func);
        }

        public void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            func.Compile().Invoke(fake.DidNotReceive());
        }

        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            return new NSubstituteMethodCallOccurance<TFake>(fake, func);
        }

        public IMatcher<TReturnValue> CreateMatcher<TReturnValue>()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}