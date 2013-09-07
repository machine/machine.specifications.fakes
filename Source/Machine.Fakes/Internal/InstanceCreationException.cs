using System;

using Machine.Specifications;

namespace Machine.Fakes.Internal
{
    /// <summary>
    /// Exception that occurs when a type cannot be instantiated by the automatic faking/dependency resolution
    /// </summary>
    public class InstanceCreationException : SpecificationException
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type that cannot be created</param>
        /// <param name="reason">Reason</param>
        public InstanceCreationException(Type type, string reason)
            : base(GetMessage(type, reason))
        { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="type">Type that cannot be created</param>
        /// <param name="reason">Reason</param>
        /// <param name="inner">Inner Exception</param>
        public InstanceCreationException(Type type, string reason, Exception inner)
            : base(GetMessage(type, reason), inner)
        { }

        static string GetMessage(Type type, string reason)
        {
            return string.Format(
                "Unable to create an instance of type {0}.{1}{2}.",
                type.Name,
                Environment.NewLine,
                reason);
        }
    }
}