using System;

namespace Machine.Specifications.Fakes
{
    /// <summary>
    ///   Defines a fake framework independent fluent interface for setting up behavior
    ///   for methods returning a result (queries).
    /// </summary>
    /// <typeparam name = "TReturn">
    ///   Specifies the return value of the behavior under configuration.
    /// </typeparam>
    public interface IQueryOptions<TReturn>
    {
        /// <summary>
        ///   Sets up the return value of a behavior.
        /// </summary>
        /// <param name = "returnValue">
        ///   Specifies the return value.
        /// </param>
        void Return(TReturn returnValue);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "valueFunction" />
        ///   will be used to evaluate the result value of a behavior.
        /// </summary>
        /// <param name = "valueFunction">
        ///   Specifies the function which is called when the method is called.
        /// </param>
        /// <remarks>
        ///   Use this for configuring parameterless methods.
        /// </remarks>
        void Return(Func<TReturn> valueFunction);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "valueFunction" />
        ///   will be used to evaluate the result value of a behavior.
        /// </summary>
        /// <param name = "valueFunction">
        ///   Specifies the function which is called when the method is called.
        /// </param>
        /// <remarks>
        ///   Use this for configuring methods with a single parameter.
        /// </remarks>
        void Return<T>(Func<T, TReturn> valueFunction);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "valueFunction" />
        ///   will be used to evaluate the result value of a behavior.
        /// </summary>
        /// <param name = "valueFunction">
        ///   Specifies the function which is called when the method is called.
        /// </param>
        /// <remarks>
        ///   Use this for configuring methods with two parameters.
        /// </remarks>
        void Return<T1, T2>(Func<T1, T2, TReturn> valueFunction);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "valueFunction" />
        ///   will be used to evaluate the result value of a behavior.
        /// </summary>
        /// <param name = "valueFunction">
        ///   Specifies the function which is called when the method is called.
        /// </param>
        /// <remarks>
        ///   Use this for configuring methods with three parameters.
        /// </remarks>
        void Return<T1, T2, T3>(Func<T1, T2, T3, TReturn> valueFunction);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "valueFunction" />
        ///   will be used to evaluate the result value of a behavior.
        /// </summary>
        /// <param name = "valueFunction">
        ///   Specifies the function which is called when the method is called.
        /// </param>
        /// <remarks>
        ///   Use this for configuring methods with four parameters.
        /// </remarks>
        void Return<T1, T2, T3, T4>(Func<T1, T2, T3, T4, TReturn> valueFunction);

        /// <summary>
        ///   Configures that the invocation of the related behavior
        ///   results in the specified <see cref = "Exception" /> beeing thrown.
        /// </summary>
        /// <param name = "exception">
        ///   Specifies the exception which should be thrown when the 
        ///   behavior is invoked.
        /// </param>
        void Throw(Exception exception);
    }
}
