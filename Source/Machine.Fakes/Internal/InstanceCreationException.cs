using System;

namespace Machine.Fakes.Internal
{
    public class InstanceCreationException : Exception
    {
        public InstanceCreationException(Type type, string reason)
            : base(string.Format(
                "Unable to create an instance of type {0}.{1}{2}.",
                type.Name,
                Environment.NewLine,
                reason))
        { }
    }
}