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
            AddConverter(InlineConstraintNames.IsAny, RewriteIsAnyMethod);
            AddConverter(InlineConstraintNames.Is, RewriteIsMethod);
            AddConverter(InlineConstraintNames.IsA, RewriteIsAMethod);
            AddConverter(InlineConstraintNames.Matches, RewriteMatchesMethod);
            AddConverter(InlineConstraintNames.IsAnything, RewriteIsAnythingMember);
            AddConverter(InlineConstraintNames.IsNull, RewriteIsNullMember);
            AddConverter(InlineConstraintNames.IsNotNull, RewriteIsNotNullMember);
        }

        static Expression RewriteIsAnythingMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            return Expression.Call(
                typeof(It),
                "IsAny",
                new[] { typeArgument });
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(typeArgument, "param");
            var lamdaType = typeof(Func<,>).MakeGenericType(typeArgument, typeof(bool));

            var equalExpression = Expression.Equal(
                parameterExpression,
                Expression.Constant(null));

            var expr = Expression.Lambda(
                lamdaType,
                equalExpression,
                parameterExpression);

            return Expression.Call(
                typeof(It),
                "Is",
                new[] { typeArgument },
                expr);
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            return Expression.Call(
                typeof(It),
                "IsNotNull",
                new[] { node.Member.DeclaringType.GetFirstTypeArgument() });
        }

        Expression RewriteMatchesMethod(MethodCallExpression expression)
        {
            var method = expression.Method;
            var matchExpression = expression.Arguments[0];

            if (matchExpression is UnaryExpression)
            {
                matchExpression = ((UnaryExpression)matchExpression).Operand;
                var methodTypeParameter = method.DeclaringType.GetFirstTypeArgument();
                return Expression.Call(typeof(It), "Is", new[] { methodTypeParameter }, matchExpression);
            }

            return VisitMethodCall(expression);
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            return Expression.Call(
                typeof(It),
                "IsAny",
                new[] { expression.Method.GetFirstTypeArgument() });
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var argument = expression.Arguments[0];
            var parameter = Expression.Parameter(argument.Type, "param");

            return Expression.Call(
                typeof(It),
                "Is",
                new[] { argument.Type },
                Expression.Lambda(
                    typeof(Func<,>).MakeGenericType(argument.Type, typeof(bool)),
                    Expression.Equal(parameter, argument),
                    parameter));
        }

        static Expression RewriteIsAMethod(MethodCallExpression expression)
        {
            var derivedType = expression.Method.GetFirstTypeArgument();
            var baseType = expression.Method.DeclaringType.GetFirstTypeArgument();
            var parameterExpression = Expression.Parameter(baseType, "param");
            var lamdaType = typeof(Func<,>).MakeGenericType(baseType, typeof(bool));
            var isTypeExpression = Expression.TypeIs(parameterExpression, derivedType);
            var lambda = Expression.Lambda(lamdaType, isTypeExpression, parameterExpression);

            return Expression.Call(typeof(It), "Is", new[] { baseType }, lambda);
        }
    }
}