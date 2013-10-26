using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    /// <summary>
    /// An implementation of <see cref = "IFakeEngine" /> using the Rhino Mocks framework.
    /// </summary>
    public class RhinoFakeEngine : RewritingFakeEngine
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public RhinoFakeEngine() : base(new RhinoMocksExpressionRewriter()) { }

        /// <inheritdoc/>
        public override object CreateFake(Type interfaceType, params object[] args)
        {
            var stub = MockRepository.GenerateMock(interfaceType, new Type[0], args);
            if (!(stub is Delegate))
            {
                RhinoPropertyBehavior.RegisterPropertyBehavior((IMockedObject)stub);
            }
            stub.Replay();
            return stub;
        }

        /// <inheritdoc/>
        public override T PartialMock<T>(params object[] args)
        {
            var mock = MockRepository.GeneratePartialMock<T>(args);
            mock.Replay();
            return mock;
        }

        /// <inheritdoc/>
        protected override IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TDependency, TReturnValue>(
            TDependency fake,
            Expression<Func<TDependency, TReturnValue>> func)
        {
            if (IsPropertyAccess(func))
                RhinoPropertyBehavior.RemovePropertyBehavior(fake);

            return new RhinoQueryOptions<TReturnValue>(fake.Stub(f => func.Compile()(f)));
        }

        bool IsPropertyAccess<TDependency, TReturnValue>(Expression<Func<TDependency, TReturnValue>> func)
        {
            return !(func.Body is MethodCallExpression);
        }

        /// <inheritdoc/>
        protected override ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func)
        {
            return new RhinoCommandOptions(fake.Stub(func.Compile()));
        }

        /// <inheritdoc/>
        protected override void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func)
        {
            fake.AssertWasNotCalled(func.Compile());
        }

        /// <inheritdoc/>
        protected override IMethodCallOccurrence OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake,
            Expression<Action<TFake>> func)
        {
            return new RhinoMethodCallOccurrence<TFake>(fake, func.Compile());
        }
    }
}