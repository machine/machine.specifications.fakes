using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    /// <summary>
    ///   An implementation of <see cref = "IFakeEngine" />
    ///   using the NSubstitute framework.
    /// </summary>
    public class NSubstituteEngine : RewritingFakeEngine
    {
        public NSubstituteEngine() : base(new NSubstituteExpressionRewriter())
        {
        }

        public override object CreateFake(Type interfaceType)
        {
            return Substitute.For(new[] {interfaceType}, null);
        }

        public override T PartialMock<T>(params object[] args) 
        {
            return Substitute.For<T>(args);
        }

        protected override IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) 
        {
            return new NSubstituteQueryOptions<TFake, TReturnValue>(fake, func);
        }

        protected override ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) 
        {
            return new NSubstituteCommandOptions<TFake>(fake, func);
        }

        protected override void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func)
        {
            func.Compile().Invoke(fake.DidNotReceive());
        }

        protected override IMethodCallOccurance OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) 
        {
            return new NSubstituteMethodCallOccurance<TFake>(fake, func);
        }
    }
}