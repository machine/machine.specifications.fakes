using System;
using System.Linq.Expressions;

namespace Machine.Fakes.Internal
{
    public interface IMatcher<TParameter>
    {
        TParameter Match(Expression<Func<TParameter, bool>> matchExpression);
    }
}