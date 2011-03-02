using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    internal class NSubstituteCommandOptions<TFake> : ICommandOptions where TFake : class
    {
        private readonly Action<TFake> _action;
        private readonly TFake _fake;

        public NSubstituteCommandOptions(TFake fake, Expression<Action<TFake>> action)
        {   
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(action, "action");

            _fake = fake;
            _action = action.Compile();
        }

        #region ICommandOptions Members

        public void Callback(Action callback)
        {
            _fake
                .When(_action)
                .Do(x => callback.Invoke());
        }

        public void Callback<T>(Action<T> callback)
        {
            _fake
                .When(_action)
                .Do(f => callback(f.Arg<T>()));
        }

        public void Callback<T1, T2>(Action<T1, T2> callback)
        {
            _fake
                .When(_action)
                .Do(f =>
                        {
                            var args = f.Args();
                            callback((T1) args[0], (T2) args[1]);
                        });
        }

        public void Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            _fake
                .When(_action)
                .Do(f =>
                        {
                            var args = f.Args();
                            callback((T1) args[0], (T2) args[1], (T3) args[2]);
                        });
        }

        public void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            _fake
                .When(_action)
                .Do(f =>
                        {
                            var args = f.Args();
                            callback((T1) args[0], (T2) args[1], (T3) args[2], (T4) args[3]);
                        });
        }

        public void Throw(Exception exception)
        {
            _fake
                .When(_action)
                .Do(f => { throw exception; });
        }

        #endregion
    }
}