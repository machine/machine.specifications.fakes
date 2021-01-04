using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    class NSubstituteQueryOptions<TFake, T> : IQueryOptions<T> where TFake : class
    {
        readonly Func<TFake, T> _expression;
        readonly TFake _fake;

        public NSubstituteQueryOptions(TFake fake, Expression<Func<TFake, T>> expression)
        {
            Guard.AgainstArgumentNull(fake, "fake");
            Guard.AgainstArgumentNull(expression, "expression");

            _fake = fake;
            _expression = expression.Compile();
        }

        T Method
        {
            get { return _expression.Invoke(_fake); }
        }

        public void Return(T returnValue)
        {
            Method.Returns(returnValue);
        }

        public void Return(Func<T> valueFunction)
        {
            Method.Returns(f => valueFunction.Invoke());
        }

        public void Return<T1>(Func<T1, T> valueFunction)
        {
            Method.Returns(f => valueFunction.Invoke(f.Arg<T1>()));
        }

        public void Return<T1, T2>(Func<T1, T2, T> valueFunction)
        {
            Method.Returns(f =>
            {
                var args = f.Args();
                return valueFunction.Invoke((T1)args[0], (T2)args[1]);
            });
        }

        public void Return<T1, T2, T3>(Func<T1, T2, T3, T> valueFunction)
        {
            Method.Returns(f =>
            {
                var args = f.Args();
                return valueFunction.Invoke((T1)args[0], (T2)args[1], (T3)args[2]);
            });
        }

        public void Return<T1, T2, T3, T4>(Func<T1, T2, T3, T4, T> valueFunction)
        {
            Method.Returns(f =>
            {
                var args = f.Args();
                return valueFunction.Invoke((T1)args[0], (T2)args[1], (T3)args[2], (T4)args[3]);
            });
        }

        public void Throw(Exception exception)
        {
            Method.Returns(x => { throw exception; });
        }
    }
}