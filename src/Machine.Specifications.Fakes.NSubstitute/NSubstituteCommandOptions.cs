using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Machine.Fakes.Sdk;

using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    internal class NSubstituteCommandOptions<TFake> : ICommandOptions where TFake : class
    {
        private readonly Action<TFake> _action;
        private readonly TFake _fake;
        readonly Expression<Action<TFake>> _expression;

        public NSubstituteCommandOptions(TFake fake, Expression<Action<TFake>> expression)
        {   
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(expression, "expression");

            _fake = fake;
            _expression = expression;
            _action = expression.Compile();
        }

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
                            callback((T1)args[0], (T2)args[1]);
                        });
        }

        public void Callback<T1, T2, T3>(Action<T1, T2, T3> callback)
        {
            _fake
                .When(_action)
                .Do(f =>
                        {
                            var args = f.Args();
                            callback((T1)args[0], (T2)args[1], (T3)args[2]);
                        });
        }

        public void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback)
        {
            _fake
                .When(_action)
                .Do(f =>
                        {
                            var args = f.Args();
                            callback((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
                        });
        }

        public void Throw(Exception exception)
        {
            _fake
                .When(_action)
                .Do(f => { throw exception; });
        }

        public ICallbackOptions AssignOutAndRefParameters(params object[] values)
        {
            _fake.When(_action).Do(callInfo =>
            {
                var parameters = ((MethodCallExpression)_expression.Body).Method.GetParameters();

                if (parameters.Count(IsOutOrRef) != values.Length)
                    throw new InvalidOperationException(
                        "A different number of values for out and ref parameters are specified than the number of out and ref parameters in the faked call.");
                
                var paramCount = -1;

                foreach (object value in values)
                {
                    do
                        paramCount++;
                    while (!IsOutOrRef(parameters[paramCount]));

                    callInfo[paramCount] = value;
                }
            });
            return this;
        }

        bool IsOutOrRef(ParameterInfo p)
        {
            return p.IsOut || p.ParameterType.IsByRef;
        }
    }
}