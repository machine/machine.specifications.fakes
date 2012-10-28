using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqMethodCallOccurrence<TFake> : IMethodCallOccurrence where TFake : class
    {
        private readonly Expression<Action<TFake>> _func;
        private readonly Mock<TFake> _mock;

        public MoqMethodCallOccurrence(Mock<TFake> mock, Expression<Action<TFake>> func)
        {
            Guard.AgainstArgumentNull(mock, "mock");
            Guard.AgainstArgumentNull(func, "func");

            _mock = mock;
            _func = func;

            _mock.Verify(func, global::Moq.Times.AtLeastOnce());
        }

        #region IMethodCallOccurrence Members

        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            _mock.Verify(_func, global::Moq.Times.Exactly(numberOfTimesTheMethodShouldHaveBeenCalled));
        }

        public void OnlyOnce()
        {
            _mock.Verify(_func, global::Moq.Times.Once());
        }

        public void Twice()
        {
            Times(2);
        }

        #endregion
    }
}