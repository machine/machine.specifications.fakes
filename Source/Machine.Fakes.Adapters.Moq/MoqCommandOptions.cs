using System;
using Machine.Fakes.Utils;
using Moq.Language.Flow;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqCommandOptions<TFake> : ICommandOptions where TFake : class
    {
        private readonly ISetup<TFake> _methodOptions;

        public MoqCommandOptions(ISetup<TFake> methodOptions)
        {
            Guard.AgainstArgumentNull(methodOptions, "methodOptions");

            _methodOptions = methodOptions;
        }

        #region ICommandOptions Members

        public void Throw(Exception exception)
        {
            _methodOptions.Throws(exception);
        }

        public void Callback(Action callback)
        {
            _methodOptions.Callback(callback);
        }

        public void Callback<T>(Action<T> callback)
        {
            _methodOptions.Callback(callback);
        }

        public void Callback<T1, T2>(Action<T1, T2> callback)
        {
            _methodOptions.Callback(callback);
        }

        public void Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            _methodOptions.Callback(callback);
        }

        public void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            _methodOptions.Callback(callback);
        }

        #endregion
    }
}