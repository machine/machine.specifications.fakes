using System;
using System.Reflection;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    public static class TypeExtensions
    {
        public static bool IsValueType(this Type type)
        {
#if NET40
            return type.IsValueType;
#else
            return type.GetTypeInfo().IsValueType;
#endif
        }
    }
}