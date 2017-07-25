using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
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
            AddConverter(InlineConstraintNames.IsNull, RewriteNullCheckingMember);
            AddConverter(InlineConstraintNames.IsNotNull, RewriteNullCheckingMember);
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
                    typeof(ArgumentConstraintManagerExtensions),
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
                typeof(ArgumentConstraintManagerExtensions),
                "IsEqualTo",
                new[] { innerExpression.Type },
                GetThatAccess(innerExpression.Type),
                innerExpression);
        }

        static Expression RewriteNullCheckingMember(MemberExpression node)
        {
            var argumentType = node.Member.DeclaringType.GetFirstTypeArgument();
            var thatAccess = GetThatAccess(argumentType);

            var isValueType = argumentType.IsValueType();
            if (isValueType)
            {
                // If we're checking a value type for nullity, it means the type
                // is Nullable<T>, and we want to find IsNull<T>. (Or IsNotNull<T>.) 
                argumentType = argumentType.GetGenericArguments().First();
            }

            var method = typeof(ArgumentConstraintManagerExtensions)
                .GetMethods()
                .Where(x => x.Name == node.Member.Name)
                .Single(x => x.GetGenericArguments().First().IsValueType() == isValueType)
                .MakeGenericMethod(argumentType);

            return Expression.Call(method, thatAccess);
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
                    typeof(ArgumentConstraintManagerExtensions),
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