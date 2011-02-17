using System;

namespace Machine.Fakes.Utils
{
    public static class Guard
    {
        public static void AgainstArgumentNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        public static void AgainstNullOrEmptyString(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
            {
                throw new ArgumentException(
                    string.Format(
                        "Argument {0} must not be null or an empty string",
                        argumentName));
            }
        }

        public static void ArgumentAssignableTo(Type argument, Type assignmentTargetType)
        {
            if (!assignmentTargetType.IsAssignableFrom(argument))
            {
                throw new ArgumentException(
                    string.Format("Type {0} is not assignable to the type {1}", 
                        argument.FullName, 
                        assignmentTargetType.FullName));
            }
        }
    }
}