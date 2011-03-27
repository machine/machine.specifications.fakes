using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    class NSubstituteMatcher<TParam> : IMatcher<TParam>
    {
        public TParam Match(Expression<Func<TParam, bool>> matchExpression)
        {
            var matcher = matchExpression.Compile();
            
            return Arg.Is<TParam>(p => matcher(p));
        }
    }
}