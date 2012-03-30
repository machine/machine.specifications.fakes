using System;
using System.Linq.Expressions;
using Machine.Fakes.Sdk;
using NSubstitute;

namespace Machine.Fakes.Adapters.NSubstitute
{
    class NSubstituteExpressionRewriter : AbstractExpressionRewriter
    {
        public NSubstituteExpressionRewriter()
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
                typeof(Arg),
                "Any",
                new[] { typeArgument });
        }

        static Expression RewriteIsNullMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(typeArgument, "param");
            var lamdaType = typeof(Predicate<>).MakeGenericType(typeArgument);

            var equalExpression = Expression.Equal(
                parameterExpression,
                Expression.Constant(null));

            var expr = Expression.Lambda(
                lamdaType,
                equalExpression,
                parameterExpression);

            return Expression.Call(
                typeof(Arg),
                "Is",
                new[] { typeArgument },
                expr);
        }

        static Expression RewriteIsNotNullMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;
            var typeArgument = declaringType.GetFirstTypeArgument();

            var parameterExpression = Expression.Parameter(typeArgument, "param");
            var lambdaType = typeof(Predicate<>).MakeGenericType(typeArgument);

            var notEqualExpression = Expression.Not(
                Expression.Equal(
                    parameterExpression,
                    Expression.Constant(null)));

            var lambda = Expression.Lambda(
                lambdaType,
                notEqualExpression,
                parameterExpression);

            return Expression.Call(typeof(Arg), "Is", new[] { typeArgument }, lambda);
        }

        static Expression RewriteIsAnyMethod(MethodCallExpression expression)
        {
            return Expression.Call(
                typeof(Arg),
                "Any",
                new[] { expression.Method.GetFirstTypeArgument() });
        }

        static Expression RewriteIsMethod(MethodCallExpression expression)
        {
            var argument = (ConstantExpression)expression.Arguments[0];
            var valueExpression = Expression.Constant(argument.Value);

            return Expression.Call(typeof(Arg), "Is", new[] { argument.Type }, valueExpression);
        }

        static Expression RewriteIsAMethod(MethodCallExpression expression)
        {
            var derivedType = expression.Method.GetFirstTypeArgument();
            var baseType = expression.Method.DeclaringType.GetFirstTypeArgument();
            var parameterExpression = Expression.Parameter(baseType, "param");
            var lamdaType = typeof(Predicate<>).MakeGenericType(baseType);
            var isTypeExpression = Expression.TypeIs(parameterExpression, derivedType);
            var lambda = Expression.Lambda(lamdaType, isTypeExpression, parameterExpression);

            return Expression.Call(typeof(Arg), "Is", new[] { baseType }, lambda);
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
                    typeof(Arg), 
                    "Is",
                    new[] { typeParameter },
                    newLambda);
            }

            return base.VisitMethodCall(expression); 
        }
    }
}