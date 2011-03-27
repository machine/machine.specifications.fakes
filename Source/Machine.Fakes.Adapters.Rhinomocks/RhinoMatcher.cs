using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using Rhino.Mocks;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    class RhinoMatcher<TParam> : IMatcher<TParam>
    {
        public TParam Match(Expression<Func<TParam, bool>> matchExpression)
        {
            var matcher = matchExpression.Compile();

            return Arg<TParam>.Matches(p => matcher(p));
        }
    }
}