using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    /// <summary>
    ///   An implementation of <see cref = "IFakeEngine" />
    ///   using the Moq framework.
    /// </summary>
    public class MoqFakeEngine : IFakeEngine
    {
        public object CreateFake(Type interfaceType)
        {
            var closedMockType = typeof (Mock<>).MakeGenericType(interfaceType);
            var objectProperty = closedMockType.GetProperty("Object", closedMockType);
            var instance = Activator.CreateInstance(closedMockType);
            return objectProperty.GetValue(instance, null);
        }

        public T PartialMock<T>(params object[] args) where T : class
        {
            var closedMockType = typeof (Mock<>).MakeGenericType(typeof (T));
            var callBaseProperty = closedMockType.GetProperty("CallBase", typeof (bool));
            var objectProperty = closedMockType.GetProperty("Object", typeof (T));
            var constructor = closedMockType.GetConstructor(new[]
            {
                typeof (object[])
            });
            var instance = constructor.Invoke(new[]
            {
                args
            });
            callBaseProperty.SetValue(instance, true, null);
            return objectProperty.GetValue(instance, null) as T;
        }

        public IQueryOptions<TReturnValue> SetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) where TFake : class
        {
            var mock = Mock.Get(fake);

            return new MoqQueryOptions<TFake, TReturnValue>(mock.Setup(func));
        }

        public ICommandOptions SetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) where TFake : class
        {
            var mock = Mock.Get(fake);

            return new MoqCommandOptions<TFake>(mock.Setup(func));
        }

        public void VerifyBehaviorWasNotExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class
        {
            var mock = Mock.Get(fake);

            mock.Verify(func, Times.Never());
        }

        public IMethodCallOccurance VerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) where TFake : class
        {
            var mock = Mock.Get(fake);

            return new MoqMethodCallOccurance<TFake>(mock, func);
        }

        public TParam Match<TParam>(Expression<Func<TParam, bool>> matchExpression)
        {
            return It.Is(matchExpression);
        }
    }
}