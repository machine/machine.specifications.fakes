#if NET40

namespace System.Reflection
{
    internal static class ReflectionPolyfillExtensions
    {

        public static Type GetTypeInfo(this Type type)
        {
            return type;
        }
    }
}

#endif
