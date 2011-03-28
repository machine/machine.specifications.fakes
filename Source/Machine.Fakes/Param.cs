using System;
using System.Linq.Expressions;

namespace Machine.Fakes
{
    /// <summary>
    /// Entry point for using inline parameter constraints with Machine.Fakes.
    /// </summary>
    public class Param
    {
        /// <summary>
        /// Configures that the parameter must match the specified value.
        /// </summary>
        /// <typeparam name="TParam">
        /// Specifies the parameter type.
        /// </typeparam>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam Is<TParam>(TParam value)
        {
            return default(TParam);
        }

        /// <summary>
        /// Configures that every parameter value matches.
        /// </summary>
        /// <typeparam name="TParam">
        /// Specifies the parameter type.
        /// </typeparam>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsAny<TParam>()
        {
            return default(TParam);
        }
    }

    /// <summary>
    /// Entry point for using inline parameter constraints with Machine.Fakes.
    /// </summary>
    /// <typeparam name="TParam">
    /// Specifies the parameters type.
    /// </typeparam>
    public class Param<TParam>
    {
        /// <summary>
        /// Matches when the parameter value is null.
        /// </summary>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsNull
        {
            get { return default(TParam); }
        }

        /// <summary>
        /// Matches when the parameter value is not null.
        /// </summary>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsNotNull
        {
            get { return default(TParam); }
        }

        /// <summary>
        /// Every parameter is matched.
        /// </summary>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsAnything
        {
            get { return default(TParam); }
        }

        /// <summary>
        /// Configures that the parameter must be of a particular type.
        /// </summary>
        /// <typeparam name="TOther">
        /// Specifies the other type.
        /// </typeparam>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsA<TOther>()
        {
            return default(TParam);
        }

        /// <summary>
        /// Configures that the parameter must match the specified predicate.
        /// </summary>
        /// <param name="matchExpression">
        /// Specifies the predicate.
        /// </param>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam Matches(Expression<Func<TParam, bool>> matchExpression)
        {
            return default(TParam);
        }
    }
}