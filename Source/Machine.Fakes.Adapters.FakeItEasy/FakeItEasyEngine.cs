using System;
using System.Linq.Expressions;
using FakeItEasy;
using Machine.Fakes.Internal;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    public class FakeItEasyEngine : IFakeEngine
    {
        public object CreateFake(Type interfaceType)
        {
            var closedFakeType = typeof(Fake<>).MakeGenericType(interfaceType);
            var objectProperty = closedFakeType.GetProperty("FakedObject", interfaceType);
            var instance = Activator.CreateInstance(closedFakeType);
            return objectProperty.GetValue(instance, null);
        }

        public T PartialMock<T>(params object[] args) where T : class
        {
            return A.Fake<T>(f => f.WithArgumentsForConstructor(args));
        }

        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake, 
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            var queryExpression = func.WrapExpression(fake);
            var updatedExpression = new FakeItEasyExpressionRewriter().Rewrite(queryExpression);

            var configuration = A.CallTo((Expression<Func<TReturnValue>>)updatedExpression);

            return new FakeItEasyQueryOptions<TReturnValue>(configuration);
        }

        public ICommandOptions SetUpCommandBehaviorFor<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            var callExpression = func.WrapExpression(fake);
            var configuration = A.CallTo(callExpression);

            return new FakeItEasyCommandOptions(configuration);
        }

        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            var callExpression = func.WrapExpression(fake);
            var configuration = A.CallTo(callExpression);

            return new FakeItEasyMethodCallOccurance(configuration);
        }

        public void VerifyBehaviorWasNotExecuted<TFake>(TFake fake, Expression<Action<TFake>> func) where TFake : class
        {
            var callExpression = func.WrapExpression(fake);
            
            A.CallTo(callExpression).MustNotHaveHappened();
        }
    }
}