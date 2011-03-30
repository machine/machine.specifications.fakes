using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    class MoqExpressionRewriter : AbstractExpressionRewriter
    {
        public MoqExpressionRewriter()
        {
            AddConverter(InlineConstaintNames.IsAny, RewriteIsAnyMethod);
            AddConverter(InlineConstaintNames.Is, RewriteIsMethod);
            AddConverter(InlineConstaintNames.IsA, RewriteIsAMethod);
            AddConverter(InlineConstaintNames.Matches, RewriteMatchesMethod);
            AddConverter(InlineConstaintNames.IsAnything, RewriteIsAnythingMember);
            AddConverter(InlineConstaintNames.IsNull, RewriteIsNullMember);
            AddConverter(InlineConstaintNames.IsNotNull, RewriteIsNotNullMember);
        }

        static Expression RewriteIsAnythingMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            return Expression.Call(
                typeof (It),
                "IsAny",
                new[] {typeArgument});
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(typeArgument, "param");
            var lamdaType = typeof (Func<,>).MakeGenericType(typeArgument, typeof (bool));

            var equalExpression = Expression.Equal(
                parameterExpression,
                Expression.Constant(null));

            var expr = Expression.Lambda(
                lamdaType,
                equalExpression,
                parameterExpression);

            return Expression.Call(
                typeof (It),
                "Is",
                new[] {typeArgument},
                expr);
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(typeArgument, "param");
            var lambdaType = typeof (Func<,>).MakeGenericType(typeArgument, typeof (bool));

            var notEqualExpression = Expression.Not(
                Expression.Equal(
                    parameterExpression,
                    Expression.Constant(null)));

            var lambda = Expression.Lambda(
                lambdaType,
                notEqualExpression,
                parameterExpression);

            return Expression.Call(typeof (It), "Is", new[] {typeArgument}, lambda);
        }

        Expression RewriteMatchesMethod(MethodCallExpression expression)
        {
            var method = expression.Method;
            var matchExpression = expression.Arguments[0];

            if (matchExpression is UnaryExpression)
            {
                matchExpression = ((UnaryExpression) matchExpression).Operand;
                var methodTypeParameter = method.DeclaringType.GetFirstTypeArgument();
                return Expression.Call(typeof(It), "Is", new[] { methodTypeParameter }, matchExpression);
            }

            return base.VisitMethodCall(expression);
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            return Expression.Call(
                typeof (It),
                "IsAny",
                new[] {expression.Method.GetFirstTypeArgument()});
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var argument = (ConstantExpression) expression.Arguments[0];
            var parameterExpression = Expression.Parameter(argument.Type, "param");
            var lambdaType = typeof (Func<,>).MakeGenericType(argument.Type, typeof (bool));

            var equalExpression = Expression.Equal(
                parameterExpression,
                Expression.Constant(argument.Value));

            var expr = Expression.Lambda(lambdaType, equalExpression, parameterExpression);

            return Expression.Call(typeof (It), "Is", new[] {argument.Type}, expr);
        }

        static Expression RewriteIsAMethod(MethodCallExpression expression)
        {
            var derivedType = expression.Method.GetFirstTypeArgument();
            var baseType = expression.Method.DeclaringType.GetFirstTypeArgument();
            var parameterExpression = Expression.Parameter(baseType, "param");
            var lamdaType = typeof (Func<,>).MakeGenericType(baseType, typeof (bool));
            var isTypeExpression = Expression.TypeIs(parameterExpression, derivedType);
            var lambda = Expression.Lambda(lamdaType, isTypeExpression, parameterExpression);

            return Expression.Call(typeof (It), "Is", new[] {baseType}, lambda);
        }
    }
}