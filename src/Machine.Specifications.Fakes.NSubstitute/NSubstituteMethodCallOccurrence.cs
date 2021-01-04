using System;
using System.Linq.Expressions;
using Machine.Specifications.Fakes.Sdk;
using NSubstitute;

namespace Machine.Specifications.Fakes.Adapters.NSubstitute
{
    internal class NSubstituteMethodCallOccurrence<TFake> : IMethodCallOccurrence where TFake : class
    {
        private readonly TFake _fake;
        private readonly Expression<Action<TFake>> _func;

        public NSubstituteMethodCallOccurrence(TFake fake, Expression<Action<TFake>> func)
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(func, "func");

            _fake = fake;
            _func = func;

            _func.Compile().Invoke(_fake.Received());
        }

        #region IMethodCallOccurrence Members
        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            _func.Compile().Invoke(_fake.Received(numberOfTimesTheMethodShouldHaveBeenCalled));
        }

        public void OnlyOnce()
        {
            Times(1);
        }

        public void Twice()
        {
            Times(2);
        }
        #endregion
    }
}
