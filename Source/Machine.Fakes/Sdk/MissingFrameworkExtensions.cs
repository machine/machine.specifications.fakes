using System;
using System.Collections.Generic;

namespace Machine.Fakes.Sdk
{
    internal static class MissingFrameworkExtensions
    {
        public static void Each<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T obj in enumerable)
                action(obj);
        }
    }
}