using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Machine.Fakes.Sdk;
using NSubstitute;

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
            if (CountCalls() < numberOfTimesTheMethodShouldHaveBeenCalled)
                throw new Exception();
        }

        public void OnlyOnce()
        {
            if (CountCalls() != 1)
                throw new Exception();
        }

        private int CountCalls()
        {
            var method = ((MethodCallExpression) _func.Body).Method;
            return _fake
                .ReceivedCalls()
                .Select(x => x.GetMethodInfo())
                .Where(x => x == method)
                .Count();
        }

        public void Twice()
        {
            Times(2);
        }

        #endregion
    }
}