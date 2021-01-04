using System;
using System.Linq.Expressions;
using Machine.Specifications.Fakes.Sdk;
using NSubstitute;

namespace Machine.Specifications.Fakes.Adapters.NSubstitute
{
    /// <summary>
    ///   An implementation of <see cref = "IFakeEngine" />
    ///   using the NSubstitute framework.
    /// </summary>
    public class NSubstituteEngine : RewritingFakeEngine
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public NSubstituteEngine() : base(new NSubstituteExpressionRewriter())
        {
        }

        /// <inheritdoc/>
        public override object CreateFake(Type interfaceType, params object[] args)
        {
            return Substitute.For(new[] { interfaceType }, args);
        }

        /// <inheritdoc/>
        public override T PartialMock<T>(params object[] args) 
        {
            return Substitute.For<T>(args);
        }

        /// <inheritdoc/>
        protected override IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) 
        {
            return new NSubstituteQueryOptions<TFake, TReturnValue>(fake, func);
        }

        /// <inheritdoc/>
        protected override ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) 
        {
            return new NSubstituteCommandOptions<TFake>(fake, func);
        }

        /// <inheritdoc/>
        protected override void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func)
        {
            func.Compile().Invoke(fake.DidNotReceive());
        }

        /// <inheritdoc/>
        protected override IMethodCallOccurrence OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) 
        {
            return new NSubstituteMethodCallOccurrence<TFake>(fake, func);
        }
    }
}
