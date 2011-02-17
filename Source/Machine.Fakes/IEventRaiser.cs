using System;

namespace Machine.Fakes
{
    /// <summary>
    /// This abstraction represents a single event on a fake.
    /// </summary>
    public interface IEventRaiser
    {
        /// <summary>
        /// Raises the represented event with the specified arguments.
        /// </summary>
        /// <param name="sender">
        /// Specifies the sender.
        /// </param>
        /// <param name="e">
        /// Specifies event arguments.
        /// </param>
        void Raise(object sender, EventArgs e);

        /// <summary>
        /// Raises the represented event with the specified arguments.
        /// </summary>
        /// <param name="args">
        /// Specifies the arguments as an untyped object array.
        /// </param>
        void Raise(params object[] args);
    }
}