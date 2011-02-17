using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    public static class ExpressionExtensions
    {
        public static Expression<Func<TReturn>> WrapExpression<TFake, TReturn>(
            this Expression<Func<TFake, TReturn>> expression,
            TFake fake)
        {
            var wrappedExpr = Wrap(fake, expression);

            return (Expression<Func<TReturn>>)Expression.Lambda(wrappedExpr);
        }

        public static Expression<Action> WrapExpression<TFake>(
            this Expression<Action<TFake>> expression,
            TFake fake)
        {
            var wrappedExpr = Wrap(fake, expression);

            return Expression.Lambda<Action>(wrappedExpr);
        }

        static Expression Wrap<TFake, TDelegate>(TFake fake, Expression<TDelegate> expression)
        {
            var fakeExpression = Expression.Constant(fake, typeof(TFake));
            Expression call = null;

            if (expression.Body is MethodCallExpression)
            {
                var methodExpression = (MethodCallExpression)expression.Body;
                var method = methodExpression.Method;
                call = Expression.Call(fakeExpression, method, methodExpression.Arguments);
            }

            if (expression.Body is MemberExpression)
            {
                var memberExpression = (MemberExpression)expression.Body;
                var member = (PropertyInfo)memberExpression.Member;

                call = Expression.Property(fakeExpression, member);
            }

            if (call == null)
            {
                throw new InvalidOperationException("Expresion is not pointing to a method or property");
            }

            return call;
        }
    }
}