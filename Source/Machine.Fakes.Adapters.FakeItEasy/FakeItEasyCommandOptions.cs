using System;
using FakeItEasy.Configuration;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    class FakeItEasyCommandOptions : ICommandOptions
    {
        readonly IVoidArgumentValidationConfiguration _configuration;

        public FakeItEasyCommandOptions(IVoidArgumentValidationConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Callback(Action callback)
        {
            _configuration.Invokes(f => callback());
        }

        public void Callback<T>(Action<T> callback)
        {
            _configuration.Invokes(f => callback((T)f.Arguments[0]));
        }

        public void Callback<T1, T2>(Action<T1, T2> callback)
        {
            _configuration.Invokes(f => callback(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1]));
        }

        public void Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            _configuration.Invokes(f => callback(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1],
                (T3)f.Arguments[2])); 
        }

        public void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            _configuration.Invokes(f => callback(
                (T1)f.Arguments[0],
                (T2)f.Arguments[1],
                (T3)f.Arguments[2],
                (T4)f.Arguments[3])); 
        }

        public void Throw(Exception exception)
        {
            _configuration.Throws(exception);
        }
    }
}