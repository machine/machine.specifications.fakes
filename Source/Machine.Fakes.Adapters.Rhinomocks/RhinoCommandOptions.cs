using System;
using Machine.Fakes.Sdk;
using Rhino.Mocks.Interfaces;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    class RhinoCommandOptions : ICommandOptions
    {
        private readonly IMethodOptions<object> _methodOptions;

        public RhinoCommandOptions(IMethodOptions<object> methodOptions)
        {
            Guard.AgainstArgumentNull(methodOptions, "methodOptions");

            _methodOptions = methodOptions;
        }

        public void Throw(Exception exception)
        {
            _methodOptions.Throw(exception);
        }

        public ICallbackOptions AssignOutAndRefParameters(params object[] values)
        {
            throw new NotSupportedException("The Machine.Fakes RhinoMocks adapter does not support setting up out or ref parameters. Please use FakeItEasy or NSubstitute instead.");
        }

        public void Callback(Action callback)
        {
            _methodOptions.Callback(() =>
            {
                callback();
                return true;
            });
        }

        public void Callback<T>(Action<T> callback)
        {
            _methodOptions.Callback<T>(p =>
            {
                callback(p);
                return true;
            });
        }

        public void Callback<T1, T2>(Action<T1, T2> callback)
        {
            _methodOptions.Callback<T1, T2>((p1, p2) =>
            {
                callback(p1, p2);
                return true;
            });
        }

        public void Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            _methodOptions.Callback<T1, T2, T3>((p1, p2, p3) =>
            {
                callback(p1, p2, p3);
                return true;
            });
        }

        public void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            _methodOptions.Callback<T1, T2, T3, T4>((p1, p2, p3, p4) =>
            {
                callback(p1, p2, p3, p4);
                return true;
            });
        }
    }
}