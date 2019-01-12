using System;

namespace Machine.Fakes
{
    /// <summary>
    /// Interface for setting up callbacks for methods returning void (commands).
    /// </summary>
    public interface ICallbackOptions
    {
        /// <summary>
        ///   Configures that the function supplied by <paramref name = "callback" />
        ///   will be called when the method under configuration is called.
        /// </summary>
        /// <param name = "callback">
        ///   Specifies the function which is called when the method under configuration is called.
        /// </param>
        /// <remarks>
        ///   Use this overload when you're not interested in the parameters.
        /// </remarks>
        void Callback(Action callback);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "callback" />
        ///   will be called when the method under configuration is called.
        /// </summary>
        /// <param name = "callback">
        ///   Specifies the function which is called when the method under configuration is called.
        /// </param>
        /// <remarks>
        ///   Use this for callbacks on methods with a single parameter.
        /// </remarks>
        void Callback<T>(Action<T> callback);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "callback" />
        ///   will be called when the method under configuration is called.
        /// </summary>
        /// <param name = "callback">
        ///   Specifies the function which is called when the method under configuration is called.
        /// </param>
        /// <remarks>
        ///   Use this for callbacks on methods with two parameters.
        /// </remarks>
        void Callback<T1, T2>(Action<T1, T2> callback);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "callback" />
        ///   will be called when the method under configuration is called.
        /// </summary>
        /// <param name = "callback">
        ///   Specifies the function which is called when the method under configuration is called.
        /// </param>
        /// <remarks>
        ///   Use this for callbacks on methods with three parameters.
        /// </remarks>
        void Callback<T1, T2, T3>(Action<T1, T2, T3> callback);

        /// <summary>
        ///   Configures that the function supplied by <paramref name = "callback" />
        ///   will be called when the method under configuration is called.
        /// </summary>
        /// <param name = "callback">
        ///   Specifies the function which is called when the method under configuration is called.
        /// </param>
        /// <remarks>
        ///   Use this for callbacks on methods with four parameters.
        /// </remarks>
        void Callback<T1, T2, T3, T4>(Action<T1, T2, T3, T4> callback);
    }
}