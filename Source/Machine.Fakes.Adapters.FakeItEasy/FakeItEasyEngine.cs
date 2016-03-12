using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using FakeItEasy;
using FakeItEasy.Creation;

using Machine.Fakes.Sdk;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    /// <summary>
    /// An implementation of <see cref = "IFakeEngine" />  using the FakeItEasy framework.
    /// </summary>
    public class FakeItEasyEngine : RewritingFakeEngine
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FakeItEasyEngine() : base(new FakeItEasyExpressionRewriter()) { }

        /// <inheritdoc/>
        public override object CreateFake(Type interfaceType, params object[] args)
        {
            var closedFakeType = typeof(Fake<>).MakeGenericType(interfaceType);
            var objectProperty = closedFakeType.GetProperty("FakedObject", interfaceType);

            var options = args != null && args.Length > 0
                ? CreateOptionsFor(interfaceType, args) : null;

            var instance = args != null && args.Length > 0
                ? Activator.CreateInstance(closedFakeType, new object[] { options })
                : Activator.CreateInstance(closedFakeType);

            return objectProperty.GetValue(instance, null);
        }

        Delegate CreateOptionsFor(Type type, IEnumerable ctorArgs)
        {
            var optType = typeof(IFakeOptions<>).MakeGenericType(new[] { type });
            var actType = typeof(Action<>).MakeGenericType(new[] { optType });

            var r = Expression.Parameter(optType, "r");

            return Expression.Lambda(
                actType,
                Expression.Call(
                    r,
                    optType.GetMethod("WithArgumentsForConstructor", new[] { typeof(IEnumerable<object>) }),
                    Expression.Constant(ctorArgs)),
                new[] { r }).Compile();
        }

        /// <inheritdoc/>
        public override T PartialMock<T>(params object[] args) 
        {
            return A.Fake<T>(f => f.WithArgumentsForConstructor(args));
        }

        /// <inheritdoc/>
        protected override IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake, 
            Expression<Func<TFake, TReturnValue>> func) 
        {
            var queryExpression = func.WrapExpression(fake);

            var configuration = A.CallTo(queryExpression);

            return new FakeItEasyQueryOptions<TReturnValue>(configuration);
        }

        /// <inheritdoc/>
        protected override ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) 
        {
            var callExpression = func.WrapExpression(fake);
            var configuration = A.CallTo(callExpression);

            return new FakeItEasyCommandOptions(configuration);
        }

        /// <inheritdoc/>
        protected override IMethodCallOccurrence OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) 
        {
            var callExpression = func.WrapExpression(fake);
            var configuration = A.CallTo(callExpression);

            return new FakeItEasyMethodCallOccurrence(configuration);
        }

        /// <inheritdoc/>
        protected override void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) 
        {
            var callExpression = func.WrapExpression(fake);
            
            A.CallTo(callExpression).MustNotHaveHappened();
        }
    }
}