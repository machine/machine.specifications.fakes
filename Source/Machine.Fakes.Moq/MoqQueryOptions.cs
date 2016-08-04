using System;
using Machine.Fakes.Sdk;
using Moq.Language.Flow;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqQueryOptions<TFake, TReturnValue> : IQueryOptions<TReturnValue> where TFake : class
    {
        private readonly ISetup<TFake, TReturnValue> _methodOptions;

        public MoqQueryOptions(ISetup<TFake, TReturnValue> methodOptions)
        {
            Guard.AgainstArgumentNull(methodOptions, "methodOptions");

            _methodOptions = methodOptions;
        }

        public void Return(TReturnValue returnValue)
        {
            _methodOptions.Returns(returnValue);
        }

        public void Return(Func<TReturnValue> valueFunction)
        {
            _methodOptions.Returns(valueFunction);
        }

        public void Return<T>(Func<T, TReturnValue> valueFunction)
        {
            _methodOptions.Returns(valueFunction);
        }

        public void Return<T1, T2>(Func<T1, T2, TReturnValue> valueFunction)
        {
            _methodOptions.Returns(valueFunction);
        }

        public void Return<T1, T2, T3>(Func<T1, T2, T3, TReturnValue> valueFunction)
        {
            _methodOptions.Returns(valueFunction);
        }

        public void Return<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TReturnValue> valueFunction)
        {
            _methodOptions.Returns(valueFunction);
        }

        public void Throw(Exception exception)
        {
            _methodOptions.Throws(exception);
        }
    }
}