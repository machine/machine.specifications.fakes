using System;

namespace Machine.Fakes
{
    /// <summary>
    ///   Defines a fake framework independent fluent interface for setting up behavior
    ///   for methods returning void (commands)
    /// </summary>
    public interface ICommandOptions : ICallbackOptions
    {
        /// <summary>
        ///   Configures that the invocation of the related behavior
        ///   results in the specified <see cref = "Exception" /> beeing thrown.
        /// </summary>
        /// <param name = "exception">
        ///   Specifies the exception which should be thrown when the 
        ///   behavior is invoked.
        /// </param>
        void Throw(Exception exception);

        /// <summary>
        /// Configures that the out and ref parameters of the method are set to the specified <paramref name="values"/>.
        /// </summary>
        /// <param name="values">Values to be set. Specify the values in the order the ref and out parameters appear in the method signature,
        /// any non out and ref parameters are ignored.</param>
        /// <returns>Interface for configuring further behavior</returns>
        /// <remarks>An <see cref="InvalidOperationException"/> is thrown when the method is invoked and if
        /// more or less values are given than there are out and ref parameters in the method signature.</remarks>
        ICallbackOptions AssignOutAndRefParameters(params object[] values);
    }
}