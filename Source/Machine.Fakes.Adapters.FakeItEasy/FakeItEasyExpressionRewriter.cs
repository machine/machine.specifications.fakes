using System;
using System.Linq.Expressions;
using FakeItEasy;

using Machine.Fakes.Sdk;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    /// <summary>
    /// Functionality to transform the inline constraint format used by Machine.Fakes
    /// into the specific format used by FakeItEasy.
    /// </summary>
    public class FakeItEasyExpressionRewriter : AbstractExpressionRewriter
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public FakeItEasyExpressionRewriter()
        {
            AddConverter(InlineConstraintNames.IsAny, RewriteIsAnyMethod);
            AddConverter(InlineConstraintNames.Is, RewriteIsMethod);
            AddConverter(InlineConstraintNames.IsA, RewriteIsAMethod);
            AddConverter(InlineConstraintNames.Matches, RewriteMatchesMethod);
            AddConverter(InlineConstraintNames.IsAnything, RewriteIsAnythingMember);
            AddConverter(InlineConstraintNames.IsNull, RewriteIsNullMember);
            AddConverter(InlineConstraintNames.IsNotNull, RewriteIsNotNullMember);
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

                return Expression.Call(
                    typeof(ArgumentConstraintExtensions),
                    "Matches",
                    new[] { argumentType },
                    thatExpression,
                    predicate);
            }

            return VisitMethodCall(expression);
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var innerExpression = expression.Arguments[0];

            return Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsEqualTo",
                new[] { innerExpression.Type },
                GetThatAccess(innerExpression.Type),
                innerExpression);
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();
            var thatAccess = GetThatAccess(argumentType);

            return Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsNull",
                new[] { argumentType },
                thatAccess);
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();
            var thatAccess = GetThatAccess(argumentType);

            var notAccess = typeof(IArgumentConstraintManager<>)
                .MakeGenericType(argumentType)
                .MakePropertyAccess("Not", thatAccess);

            return Expression.Call(
                typeof(ArgumentConstraintExtensions),
                "IsNull",
                new[] { argumentType },
                notAccess);
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            var argumentType = expression.Method.GetFirstTypeArgument();

            return typeof(A<>)
                .MakeGenericType(argumentType)
                .MakeStaticPropertyAccess("Ignored");
        }

        static Expression RewriteIsAnythingMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();

            return typeof(A<>)
                .MakeGenericType(argumentType)
                .MakeStaticPropertyAccess("Ignored");
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

            return Expression.Call(
                    typeof(ArgumentConstraintExtensions),
                    "Matches",
                    new[] { baseType },
                    thatExpression,
                    lambda);
        }

        static Expression GetThatAccess(Type typeArgument)
        {
            return typeof(A<>)
                .MakeGenericType(typeArgument)
                .MakeStaticPropertyAccess("That");
        }
    }
}