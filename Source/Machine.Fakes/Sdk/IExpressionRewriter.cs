using System.Linq.Expressions;

namespace Machine.Fakes.Sdk
{
    /// <summary>
    /// Rewriter abstractio that is used by Machine.Fakes
    /// in order to transform the inline constaint format used by Machine.Fakes
    /// into the specific formats used by the different mock frameworks.
    /// </summary>
    public interface IExpressionRewriter
    {
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
        Expression Rewrite(Expression expr);
    }
}