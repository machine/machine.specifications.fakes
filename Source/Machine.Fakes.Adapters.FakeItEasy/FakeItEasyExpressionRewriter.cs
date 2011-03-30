using System;
using System.Linq.Expressions;
using FakeItEasy;
using FakeItEasy.Expressions;
using Machine.Fakes.Sdk;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    public class FakeItEasyExpressionRewriter : AbstractExpressionRewriter
    {
        public FakeItEasyExpressionRewriter()
        {
            AddConverter(InlineConstaintNames.IsAny, RewriteIsAnyMethod);
            AddConverter(InlineConstaintNames.Is, RewriteIsMethod);
            AddConverter(InlineConstaintNames.IsA, RewriteIsAMethod);
            AddConverter(InlineConstaintNames.Matches, RewriteMatchesMethod);
            AddConverter(InlineConstaintNames.IsAnything, RewriteIsAnythingMember);
            AddConverter(InlineConstaintNames.IsNull, RewriteIsNullMember);
            AddConverter(InlineConstaintNames.IsNotNull, RewriteIsNotNullMember);
        }

        Expression RewriteMatchesMethod(MethodCallExpression expression)
        {
            var method = expression.Method;
            var matchExpression = expression.Arguments[0];

            if (matchExpression is UnaryExpression)
            {
                var argumentType = method.DeclaringType.GetFirstTypeArgument();
                var predicate = ((UnaryExpression)matchExpression).Operand;

                var thatExpression = GetThatAccess(argumentType);

                var methodCallExpression = Expression.Call(
                    thatExpression,
                    "Matches",
                    new Type[] { },
                    predicate);

                return ArgumentPropertyAccess(argumentType, methodCallExpression); 
            }

            return base.VisitMethodCall(expression);
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var argument = (ConstantExpression)expression.Arguments[0];
            var valueExpression = Expression.Constant(argument.Value);
            var thatAccess = GetThatAccess(argument.Type);

            var methodCallExpression = Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsEqualTo",
                new[] { argument.Type },
                thatAccess,
                valueExpression);

            return ArgumentPropertyAccess(argument.Type, methodCallExpression); 
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();
            var thatAccess = GetThatAccess(argumentType);

            var methodCallExpression = Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsNull",
                new[] { argumentType },
                thatAccess);

            return ArgumentPropertyAccess(argumentType, methodCallExpression);
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();
            var thatAccess = GetThatAccess(argumentType);

            var notAccess = typeof(ArgumentConstraintScope<>)
                .MakeGenericType(argumentType)
                .MakePropertyAccess("Not", thatAccess);

            var methodCallExpression = Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsNull",
                new[] { argumentType },
                notAccess);

            return ArgumentPropertyAccess(argumentType, methodCallExpression);
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            var argumentType = expression.Method.GetFirstTypeArgument();

            var ignoredAccess = typeof(A<>)
                .MakeGenericType(argumentType)
                .MakeStaticPropertyAccess("Ignored");

            return ArgumentPropertyAccess(argumentType, ignoredAccess);
        }

        static Expression RewriteIsAnythingMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();

            var ignoredAccess = typeof(A<>)
                .MakeGenericType(argumentType)
                .MakeStaticPropertyAccess("Ignored");

            return ArgumentPropertyAccess(argumentType, ignoredAccess);
        }

        static Expression RewriteIsAMethod(MethodCallExpression expression)
        {
            var derivedType = expression.Method.GetFirstTypeArgument();
            var baseType = expression.Method.DeclaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(baseType, "param");
            var lambdaType = typeof(Func<,>).MakeGenericType(baseType, typeof(bool));
            var isTypeExpression = Expression.TypeIs(parameterExpression, derivedType);
            var lambda = Expression.Lambda(lambdaType, isTypeExpression, parameterExpression);

            var thatExpression = GetThatAccess(baseType);

            var methodCallExpression = Expression.Call(
                thatExpression,
                "Matches",
                new Type[] { },
                lambda);

            return ArgumentPropertyAccess(baseType, methodCallExpression);
        }

        static Expression GetThatAccess(Type typeArgument)
        {
            return typeof(A<>)
                .MakeGenericType(typeArgument)
                .MakeStaticPropertyAccess("That");
        }

        static Expression ArgumentPropertyAccess(Type argumentType, Expression expression)
        {
            return typeof(ArgumentConstraint<>)
                .MakeGenericType(argumentType)
                .MakePropertyAccess("Argument", expression);
        }
    }
}