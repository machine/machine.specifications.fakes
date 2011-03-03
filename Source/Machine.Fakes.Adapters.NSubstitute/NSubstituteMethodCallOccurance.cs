using System;
using System.Linq;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using NSubstitute;
using NSubstitute.Exceptions;

namespace Machine.Fakes.Adapters.NSubstitute
{
    internal class NSubstituteMethodCallOccurance<TFake> : IMethodCallOccurance where TFake : class
    {
        private readonly TFake _fake;
        private readonly Expression<Action<TFake>> _func;

        public NSubstituteMethodCallOccurance(TFake fake, Expression<Action<TFake>> func)
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            _fake = fake;
            _func = func;

            _func.Compile().Invoke(_fake.Received());
        }

        #region IMethodCallOccurance Members

        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            var calls = CountCalls();
            if (calls < numberOfTimesTheMethodShouldHaveBeenCalled)
                throw new CallNotReceivedException(
                    string.Format("Expected {0} calls to the method but received {1}",
                                  numberOfTimesTheMethodShouldHaveBeenCalled, calls));
        }

        public void OnlyOnce()
        {
            var calls = CountCalls();
            if (calls != 1)
                throw new CallReceivedException(
                    string.Format("Expected only 1 call to the method but received {0}", calls));
        }

        public void Twice()
        {
            Times(2);
        }

        #endregion

        private int CountCalls()
        {
            var method = ((MethodCallExpression) _func.Body).Method;
            return _fake
                .ReceivedCalls()
                .Select(x => x.GetMethodInfo())
                .Where(x => x == method)
                .Count();
        }
    }
}