using System;

namespace Machine.Specifications.Fakes.Sdk
{
    /// <summary>
    /// Container class for (as it name implies) guard clauses.
    /// </summary>
    public static class Guard
    {
        /// <summary>
        /// Verifies that the argument supplied by <paramref name="argument"/>
        /// is not <c>null</c>.
        /// </summary>
        /// <param name="argument">The object to be checked.</param>
        /// <param name="argumentName">The name of the object that will be used when raising an <see cref="ArgumentException"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="argument"/> is <c>null</c>.
        /// </exception>
        public static void AgainstArgumentNull(object argument, string argumentName)
        {
            if (argument == null)
            {
                throw new ArgumentNullException(argumentName);
            }
        }

        /// <summary>
        /// Verifies that the argument supplied by <paramref name="argument"/>
        /// is neither <c>null</c> nor an empty <see cref="String"/>.
        /// </summary>
        /// <param name="argument">The object to be checked.</param>
        /// <param name="argumentName">The name of the object that will be used when raising an <see cref="ArgumentException"/>.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="argument"/> is <c>null</c> or an empty <see cref="String"/>.
        /// </exception>
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

        /// <summary>
        /// Verifies that the type supplied by <paramref name="argument"/>
        /// is assignable to the type specified by <paramref name="assignmentTargetType"/>.
        /// </summary>
        /// <param name="argument">The type to be checked.</param>
        /// <param name="assignmentTargetType">The target type.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="argument"/> is not assignable to <paramref name="assignmentTargetType"/>.
        /// </exception>
        public static void ArgumentAssignableTo(Type argument, Type assignmentTargetType)
        {
            if (!assignmentTargetType.IsAssignableFrom(argument))
            {
                throw new ArgumentException(
                    string.Format(
                        "Type {0} is not assignable to the type {1}",
                        argument.FullName,
                        assignmentTargetType.FullName));
            }
        }
    }
}
