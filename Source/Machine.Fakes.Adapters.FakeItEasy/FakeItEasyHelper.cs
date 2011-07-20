using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FakeItEasy.Creation;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    public class FakeItEasyHelper
    {
        public static Delegate CreateForType(Type type, object[] ctorArgs)
        {
            var optType = typeof(IFakeOptionsBuilder<>).MakeGenericType(new[] { type });
            var actType = typeof(Action<>).MakeGenericType(new[] { optType });

            var r = Expression.Parameter(optType, "r");
            var method = optType.GetMethod("WithArgumentsForConstructor", new[] { typeof(IEnumerable<object>) });
            var p = Expression.Constant(ctorArgs);
            var call = Expression.Call(r, method, p);
            var lambda = Expression.Lambda(actType, call, new[] { r });
            var exp = lambda.Compile();

            return exp;
        }
    }
}