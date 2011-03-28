using System;
using System.Linq.Expressions;
using Moq;

namespace Machine.Fakes.Adapters.Moq
{
    public class ExpressionRewriter : ExpressionVisitor
    {
        public Expression Modify(Expression expression)
        {
            return Visit(expression);
        }

        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var method = node.Method;
            var declaringType = method.DeclaringType;

            if (!IsInlineConstraintMethod(declaringType))
            {
                return base.VisitMethodCall(node);
            }

            switch (method.Name)
            {
                case "IsAny":
                    return Expression.Call(
                            typeof(It),
                            "IsAny",
                            new[] { method.GetGenericArguments()[0] });

                case "Is":
                    var argument = (ConstantExpression)node.Arguments[0];

                    var parameterExpression = Expression.Parameter(argument.Type, "param");
                    var lamdaType = typeof(Func<,>).MakeGenericType(argument.Type, typeof(bool));

                    var equalExpression = Expression.Equal(
                        parameterExpression,
                        Expression.Constant(argument.Value));

                    var expr = Expression.Lambda(
                        lamdaType,
                        equalExpression,
                        parameterExpression);

                    return Expression.Call(
                        typeof(It),
                        "Is",
                        new[] { argument.Type },
                        expr);

                case "Matches":
                    
                    var matchExpression = node.Arguments[0];
                    Type methodTypeParameter = null;

                    if (matchExpression is ConstantExpression)
                    {
                        methodTypeParameter = method.GetGenericArguments()[0];
                    }

                    if (matchExpression is UnaryExpression)
                    {
                        matchExpression = ((UnaryExpression)matchExpression).Operand;
                        methodTypeParameter = method.DeclaringType.GetGenericArguments()[0];
                    }

                    return Expression.Call(
                        typeof(It),
                        "Is",
                        new[] { methodTypeParameter },
                        matchExpression);

                case "IsA":
                    var derivedType = method.GetGenericArguments()[0];
                    var baseType = method.DeclaringType.GetGenericArguments()[0];
                    var parameterExpression2 = Expression.Parameter(baseType, "param");
                    var lamdaType2 = typeof(Func<,>).MakeGenericType(baseType, typeof(bool));

                    var isTypeExpression = Expression.TypeIs(
                        parameterExpression2,
                        derivedType);

                    var expr2 = Expression.Lambda(
                        lamdaType2,
                        isTypeExpression,
                        parameterExpression2);

                    return Expression.Call(
                        typeof(It),
                        "Is",
                        new[] { baseType },
                        expr2);
                default:
                    return base.VisitMethodCall(node);
            }
        }

        protected override Expression VisitMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;

            if (declaringType.GetGenericTypeDefinition() != typeof(Param<>))
            {
                return base.VisitMember(node);
            }

            var typeArgument = declaringType.GetGenericArguments()[0];

            switch (node.Member.Name)
            {
                case "IsAnything":
                    return Expression.Call(
                            typeof(It),
                            "IsAny",
                            new[] { typeArgument });

                case "IsNull":

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

                case "IsNotNull":

                    var parameterExpression2 = Expression.Parameter(typeArgument, "param");
                    var lamdaType2 = typeof(Func<,>).MakeGenericType(typeArgument, typeof(bool));

                    var notEqualExpression = Expression.Not(
                        Expression.Equal(
                            parameterExpression2,
                            Expression.Constant(null)));

                    var expr2 = Expression.Lambda(
                        lamdaType2,
                        notEqualExpression,
                        parameterExpression2);

                    return Expression.Call(
                        typeof(It),
                        "Is",
                        new[] { typeArgument },
                        expr2);

                default:
                    return base.VisitMember(node);
            }
        }

        static bool IsInlineConstraintMethod(Type declaringType)
        {
            if (declaringType == typeof(Param))
            {
                return true;
            }

            return declaringType.IsGenericType && declaringType.GetGenericTypeDefinition() == typeof(Param<>);
        }
    }
}