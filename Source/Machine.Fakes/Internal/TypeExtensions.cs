using System;
using System.Collections.Generic;

namespace Machine.Fakes.Internal
{
    static class TypeExtensions
    {
        internal static bool IsGenericEnumerable(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(IEnumerable<>);
        }

        internal static bool IsFunc(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(Func<>);
        }

        internal static bool IsLazy(this Type type)
        {
            return type.GetGenericTypeDefinition() == typeof(Lazy<>);
        }
    }
}