using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using Machine.Fakes.Sdk;

namespace Machine.Fakes
{
    public class Param
    {
        public static TParam Matches<TParam>(Expression<Func<TParam, bool>> matchExpression)
        {
            return Param<TParam>.Matches(matchExpression);
        }

        public static TParam Is<TParam>(TParam value)
        {
            return Param<TParam>.Is(value);
        }

        public static TParam IsAny<TParam>()
        {
            return Param<TParam>.IsAnything;
        }
    }

    public class Param<TParam>
    {
        public static TParam IsNull
        {
            get { return Matches(paramValue => Equals(paramValue, null)); }
        }

        public static TParam IsNotNull
        {
            get { return Matches(paramValue => !Equals(paramValue, null)); }
        }

        public static TParam IsAnything
        {
            get { return Matches(paramValue => true); }
        }

        public static TParam Is(TParam value)
        {
            return Matches(paramValue => Equals(paramValue, value));
        }

        public static TParam IsA<TOther>()
        {
            return Matches(paramValue => paramValue is TOther);
        }

        public static TParam Matches(Expression<Func<TParam, bool>> matchExpression)
        {
            Guard.AgainstArgumentNull(matchExpression, "matchExpression");

            return FakeEngineGateway.Match(matchExpression);
        }
    }
}