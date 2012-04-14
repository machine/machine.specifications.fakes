using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    /// <summary>
    ///   An implementation of <see cref = "IFakeEngine" />
    ///   using the Moq framework.
    /// </summary>
    public class MoqFakeEngine : RewritingFakeEngine
    {
        public MoqFakeEngine() : base(new MoqExpressionRewriter())
        {
        }

        public override object CreateFake(Type interfaceType, params object[] args)
        {
            var closedMockType = typeof(Mock<>).MakeGenericType(interfaceType);
            var instance = (args != null && args.Length > 0) ? Activator.CreateInstance(closedMockType, args) : Activator.CreateInstance(closedMockType);
            var objectProperty = closedMockType.GetProperty("Object", closedMockType);

            return objectProperty.GetValue(instance, null);
        }

        public override T PartialMock<T>(params object[] args) 
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

        protected override IQueryOptions<TReturnValue> OnSetUpQueryBehaviorFor<TFake, TReturnValue>(
            TFake fake,
            Expression<Func<TFake, TReturnValue>> func) 
        {
            var mock = Mock.Get(fake);

            return new MoqQueryOptions<TFake, TReturnValue>(mock.Setup(func));
        }

        protected override ICommandOptions OnSetUpCommandBehaviorFor<TFake>(
            TFake fake,
            Expression<Action<TFake>> func) 
        {
            var mock = Mock.Get(fake);

            return new MoqCommandOptions<TFake>(mock.Setup(func));
        }

        protected override void OnVerifyBehaviorWasNotExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) 
        {
            var mock = Mock.Get(fake);

            mock.Verify(func, Times.Never());
        }

        protected override IMethodCallOccurance OnVerifyBehaviorWasExecuted<TFake>(
            TFake fake, 
            Expression<Action<TFake>> func) 
        {
            var mock = Mock.Get(fake);

            return new MoqMethodCallOccurance<TFake>(mock, func);
        }
    }
}