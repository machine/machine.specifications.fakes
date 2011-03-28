using System;
using System.Reflection;

namespace Machine.Fakes.Sdk
{
    public static class ReflectionExtensions
    {
        public static bool IsMFakesConstaintType(this Type type)
        {
            return type == typeof(Param) || type.ClosesGenericParamType();
        }

        public static bool ClosesGenericParamType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Param<>);
        }

        public static Type GetFirstTypeArgument(this Type type)
        {
            if (!type.IsGenericType)
            {
                throw new ArgumentException("Specified type is not a generic type", "type");
            }

            return type.GetGenericArguments()[0];
        }

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