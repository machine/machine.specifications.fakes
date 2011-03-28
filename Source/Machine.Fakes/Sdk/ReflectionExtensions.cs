using System;
using System.Reflection;

namespace Machine.Fakes.Sdk
{
    /// <summary>
    /// Helper class which contains all the helper method needed for reflection.
    /// </summary>
    public static class ReflectionExtensions
    {
        /// <summary>
        /// Checks whether the supplied type is one of the machine.fakes
        /// inline constraint types.
        /// </summary>
        /// <param name="type">
        /// Specifies the type to check.
        /// </param>
        /// <returns>
        /// <c>true</c> if it's one of the constraint types. Otherwise not.
        /// </returns>
        public static bool IsMFakesConstaintType(this Type type)
        {
            return type == typeof(Param) || type.ClosesGenericParamType();
        }

        /// <summary>
        /// Checks whether the supplied type closes the <see cref="Param{T}"/> class.
        /// </summary>
        /// <param name="type">
        /// Specifies the type to be checked.
        /// </param>
        /// <returns>
        /// <c>true</c> if the type closes the <see cref="Param{T}"/> type. Otherwise <c>false</c>.
        /// </returns>
        public static bool ClosesGenericParamType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Param<>);
        }

        /// <summary>
        /// Gets the first generic type argument of the specified type.
        /// </summary>
        /// <param name="type">
        /// Specifies the type to extract the type argument from.
        /// </param>
        /// <returns>
        /// The extracted type.
        /// </returns>
        public static Type GetFirstTypeArgument(this Type type)
        {
            if (!type.IsGenericType)
            {
                throw new ArgumentException("Specified type is not a generic type", "type");
            }

            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Gets the first generic type argument of the specified type.
        /// </summary>
        /// <param name="method">
        /// Specifies the method to extract the type argument from.
        /// </param>
        /// <returns>
        /// The extracted type.
        /// </returns>
        public static Type GetFirstTypeArgument(this MethodInfo method)
        {
            if (!method.IsGenericMethod)
            {
                throw new ArgumentException("Specified method is not a generic method", "method");
            }

            return method.GetGenericArguments()[0];
        }
    }
}