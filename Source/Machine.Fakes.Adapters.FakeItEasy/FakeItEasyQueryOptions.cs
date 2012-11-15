using System;

using FakeItEasy;
using FakeItEasy.Configuration;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    class FakeItEasyQueryOptions<TReturnValue> : IQueryOptions<TReturnValue>
    {
        readonly IReturnValueArgumentValidationConfiguration<TReturnValue> _configuration;

        public FakeItEasyQueryOptions(IReturnValueArgumentValidationConfiguration<TReturnValue> configuration)
        {
            _configuration = configuration;
        }

        public void Return(TReturnValue returnValue)
        {
            _configuration.ReturnsLazily(f => returnValue);
        }

        public void Return(Func<TReturnValue> valueFunction)
        {
            _configuration.ReturnsLazily(f => valueFunction());
        }

        public void Throw(Exception exception)
        {
            _configuration.Throws(exception);
        }

        public void Return<T>(Func<T, TReturnValue> valueFunction)
        {
            _configuration.ReturnsLazily(f => valueFunction((T)f.Arguments[0]));
        }

        public void Return<T1, T2>(Func<T1, T2, TReturnValue> valueFunction)
        {
            _configuration.ReturnsLazily(f => valueFunction(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1]));
        }

        public void Return<T1, T2, T3>(Func<T1, T2, T3, TReturnValue> valueFunction)
        {
            _configuration.ReturnsLazily(f => valueFunction(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1],
                (T3)f.Arguments[2]));
        }

        public void Return<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TReturnValue> valueFunction)
        {
            _configuration.ReturnsLazily(f => valueFunction(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1],
                (T3)f.Arguments[2],
                (T4)f.Arguments[3]));
        }
    }
}