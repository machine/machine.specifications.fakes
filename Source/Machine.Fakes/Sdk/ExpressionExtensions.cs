using System;
using System.Linq.Expressions;

namespace Machine.Fakes.Sdk
{
    /// <summary>
    /// A helper class for extensions on expression.
    /// </summary>
    public static class ExpressionExtensions
    {
        /// <summary>
        /// Rewrites the expression specified by <paramref name="expression"/>
        /// with the rewriter specified by <paramref name="rewriter"/>.
        /// </summary>
        /// <typeparam name="TType">
        /// Specifies the parameter type in the expression.
        /// </typeparam>
        /// <typeparam name="TValue">
        /// Specifies the return type in the expression.
        /// </typeparam>
        /// <param name="expression">
        /// Specifies the expression. 
        /// </param>
        /// <param name="rewriter">
        /// Specifies the rewriter.
        /// </param>
        /// <returns>
        /// The converted expression.
        /// </returns>
        public static Expression<Func<TType, TValue>> RewriteUsing<TType, TValue>(
            this Expression<Func<TType, TValue>> expression, 
            IExpressionRewriter rewriter)
        {
            Guard.AgainstArgumentNull(expression, "expression");
            Guard.AgainstArgumentNull(rewriter, "rewriter");

            return rewriter.Rewrite(expression) as Expression<Func<TType, TValue>>;
        }

        /// <summary>
        /// Rewrites the expression specified by <paramref name="expression"/>
        /// with the rewriter specified by <paramref name="rewriter"/>.
        /// </summary>
        /// <typeparam name="TType">
        /// Specifies the parameter type in the expression.
        /// </typeparam>
        /// <param name="expression">
        /// Specifies the expression. 
        /// </param>
        /// <param name="rewriter">
        /// Specifies the rewriter.
        /// </param>
        /// <returns>
        /// The converted expression.
        /// </returns>
        public static Expression<Action<TType>> RewriteUsing<TType>(
            this Expression<Action<TType>> expression, 
            IExpressionRewriter rewriter)
        {
            Guard.AgainstArgumentNull(expression, "expression");
            Guard.AgainstArgumentNull(rewriter, "rewriter");

            return rewriter.Rewrite(expression) as Expression<Action<TType>>;
        }
    }
}