using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Machine.Fakes.Sdk
{
    /// <summary>
    /// A converter for a <see cref="MethodCallExpression"/>.
    /// </summary>
    /// <param name="expression">
    /// Specifies the expression to be converted.
    /// </param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public delegate Expression MethodExpressionConverter(MethodCallExpression expression);

    /// <summary>
    /// A converter for a <see cref="MemberExpression"/>.
    /// </summary>
    /// <param name="expression">
    /// Specifies the expression to be converted.
    /// </param>
    /// <returns>
    /// The result of the conversion.
    /// </returns>
    public delegate Expression MemberExpressionConverter(MemberExpression expression);

    /// <summary>
    /// Base class for <see cref="IExpressionRewriter"/> implementations.
    /// </summary>
    public abstract class AbstractExpressionRewriter : ExpressionVisitor, IExpressionRewriter
    {
        private readonly ConcurrentDictionary<string, MethodExpressionConverter> _methodCallConverters = new ConcurrentDictionary<string, MethodExpressionConverter>();
        private readonly ConcurrentDictionary<string, MemberExpressionConverter> _memberAccessConverters = new ConcurrentDictionary<string, MemberExpressionConverter>();
        
        /// <summary>
        /// Rewrites the expression tree and replaces all occurences
        /// of Machine.Fakes specific inline constraints with the 
        /// equivalents of a specific mock framework.
        /// </summary>
        /// <param name="expr">
        /// Specifies the input expression.
        /// </param>
        /// <returns>
        /// Returns the target expression in the format of the relevant mock framework.
        /// </returns>
        public Expression Rewrite(Expression expr)
        {
            return Visit(expr);
        }

        /// <summary>
        /// Adds a converter for a particular method.
        /// </summary>
        /// <param name="methodName">
        /// Specifies the name of the method.
        /// </param>
        /// <param name="converter">
        /// Specifies the converter
        /// </param>
        protected void AddConverter(string methodName, MethodExpressionConverter converter)
        {
            Guard.AgainstArgumentNull(methodName, "methodName");
            Guard.AgainstArgumentNull(converter, "converter");

            _methodCallConverters.AddOrUpdate(methodName, converter, (m, c) => converter);
        }

        /// <summary>
        /// Adds a converter for a particular member.
        /// </summary>
        /// <param name="methodName">
        /// Specifies the name of the member.
        /// </param>
        /// <param name="converter">
        /// Specifies the converter
        /// </param>
        protected void AddConverter(string methodName, MemberExpressionConverter converter)
        {
            Guard.AgainstArgumentNull(methodName, "methodName");
            Guard.AgainstArgumentNull(converter, "converter");

            _memberAccessConverters.AddOrUpdate(methodName, converter, (m, c) => converter);
        }

        /// <summary>
        /// Is called on each <see cref="MethodCallExpression"/> expression.
        /// </summary>
        /// <param name="node">
        /// Specifies the expression node.
        /// </param>
        /// <returns>
        /// The new expression to be used for that node.
        /// </returns>
        protected override Expression VisitMethodCall(MethodCallExpression node)
        {
            var method = node.Method;
            var declaringType = method.DeclaringType;

            if (!declaringType.IsMFakesConstraint())
            {
                return base.VisitMethodCall(node);
            }

            return _methodCallConverters.GetOrAdd(method.Name, base.VisitMethodCall)(node);
        }

        /// <summary>
        /// Is called on each <see cref="MemberExpression"/> expression.
        /// </summary>
        /// <param name="node">
        /// Specifies the expression node.
        /// </param>
        /// <returns>
        /// The new expression to be used for that node.
        /// </returns>
        protected override Expression VisitMember(MemberExpression node)
        {
            var declaringType = node.Member.DeclaringType;

            if (!declaringType.ClosesGenericParamType())
            {
                return base.VisitMember(node);
            }

            return _memberAccessConverters
                .GetOrAdd(node.Member.Name, base.VisitMember)(node);
        }
    }
}