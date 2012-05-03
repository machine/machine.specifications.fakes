using System;
using System.Linq.Expressions;
using System.Reflection;
using Machine.Fakes.Sdk;
using Rhino.Mocks;
using Rhino.Mocks.Constraints;

namespace Machine.Fakes.Adapters.Rhinomocks
{
    class RhinoMocksExpressionRewriter : AbstractExpressionRewriter
    {
        public RhinoMocksExpressionRewriter()
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
                var typeParameter = method.DeclaringType.GetFirstTypeArgument();
                var oldLambda = (LambdaExpression)((UnaryExpression)matchExpression).Operand;

                var newLambda = Expression.Lambda(
                    typeof(Predicate<>).MakeGenericType(typeParameter),
                    oldLambda.Body,
                    oldLambda.Parameters);

                return Expression.Call(
                    typeof(Arg<>).MakeGenericType(typeParameter),
                    "Matches",
                    new Type[] { },
                    newLambda);
            }

            return VisitMethodCall(expression);
        }

        static Expression RewriteIsAMethod(MethodCallExpression expression)
        {
            var derivedType = expression.Method.GetFirstTypeArgument();
            var baseType = expression.Method.DeclaringType.GetFirstTypeArgument();
            var parameterExpression = Expression.Parameter(baseType, "param");
            var lamdaType = typeof(Predicate<>).MakeGenericType(baseType);
            var isTypeExpression = Expression.TypeIs(parameterExpression, derivedType);
            var lambda = Expression.Lambda(lamdaType, isTypeExpression, parameterExpression);

            return Expression.Call(
                typeof(Arg<>).MakeGenericType(baseType),
                "Matches",
                new Type[] { },
                lambda);
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var argument = (ConstantExpression)expression.Arguments[0];
            var valueExpression = Expression.Constant(argument.Value);

            return Expression.Call(typeof(Arg), "Is", new[] { argument.Type }, valueExpression);
        }

        static Expression RewriteIsAnythingMember(MemberExpression node)
        {
            var typeArgument = node.Member.DeclaringType.GetFirstTypeArgument();
            return AccessIsProperty(typeArgument, "Anything");
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var typeArgument = node.Member.DeclaringType.GetFirstTypeArgument();
            return AccessIsProperty(typeArgument, "Null");
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            var typeArgument = node.Member.DeclaringType.GetFirstTypeArgument();
            return AccessIsProperty(typeArgument, "NotNull");
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            var typeArgument = expression.Method.GetFirstTypeArgument();
            return AccessIsProperty(typeArgument, "Anything");
        }

        static MemberExpression AccessIsProperty(Type typeArgument, string property)
        {
            var closedArgumentType = typeof(Arg<>).MakeGenericType(typeArgument);
            var isProperty = closedArgumentType.GetProperty("Is", BindingFlags.Public | BindingFlags.Static);
            var isPropertyAccess = Expression.MakeMemberAccess(null, isProperty);

            var targetProperty = typeof(IsArg<>)
                .MakeGenericType(typeArgument)
                .GetProperty(property, BindingFlags.Public | BindingFlags.Instance);

            return Expression.MakeMemberAccess(isPropertyAccess, targetProperty);
        }
    }
}