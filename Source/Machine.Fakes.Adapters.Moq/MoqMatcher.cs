using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqMatcher<TParam> : IMatcher<TParam>
    {
        public TParam Match(Expression<Func<TParam, bool>> matchExpression)
        {
            return It.Is(matchExpression);
        }
    }
}