using System;
using System.Linq.Expressions;
using Machine.Fakes.Internal;
using Machine.Fakes.Sdk;

namespace Machine.Fakes
{
    /// <summary>
    /// Entry point for using inline parameter constraints with Machine.Fakes.
    /// </summary>
    public class Param
    {
        /// <summary>
        /// Configures that the parameter must match the specified predicate.
        /// </summary>
        /// <typeparam name="TParam">
        /// Specifies the parameter type.
        /// </typeparam>
        /// <param name="matchExpression">
        /// Specifies the predicate.
        /// </param>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam Matches<TParam>(Expression<Func<TParam, bool>> matchExpression)
        {
            return Param<TParam>.Matches(matchExpression);
        }

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
            return Param<TParam>.Is(value);
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
            return Param<TParam>.IsAnything;
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
            get { return Matches(paramValue => Equals(paramValue, null)); }
        }

        /// <summary>
        /// Matches when the parameter value is not null.
        /// </summary>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsNotNull
        {
            get { return Matches(paramValue => !Equals(paramValue, null)); }
        }

        /// <summary>
        /// Every parameter is matched.
        /// </summary>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam IsAnything
        {
            get { return Matches(paramValue => true); }
        }

        /// <summary>
        /// Configures that the parameter must match the specified value.
        /// </summary>
        /// <param name="value">
        /// Specifies the value on which is matched.
        /// </param>
        /// <returns>
        /// A substitute type that isn't directly used.
        /// </returns>
        public static TParam Is(TParam value)
        {
            return Matches(paramValue => Equals(paramValue, value));
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
            return Matches(paramValue => paramValue is TOther);
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
            Guard.AgainstArgumentNull(matchExpression, "matchExpression");

            return FakeEngineGateway.Match(matchExpression);
        }
    }
}