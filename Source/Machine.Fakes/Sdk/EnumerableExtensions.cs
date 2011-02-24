using System;
using System.Collections.Generic;
using System.Linq;

namespace Machine.Fakes.Sdk
{
    static class EnumerableExtensions
    {
        public static T FirstOrCustomDefaultValue<T>(this IEnumerable<T> enumerable, T customDefaultValue)
        {
            Guard.AgainstArgumentNull(enumerable, "enumerable");

            var collection = enumerable.AlternativeIfNullOrEmpty(Enumerable.Empty<T>);
            var firstValue = collection.FirstOrDefault();

            return Equals(firstValue, default(T)) ? customDefaultValue : firstValue;
        }

        public static IEnumerable<T> AlternativeIfNullOrEmpty<T>(
            this IEnumerable<T> enumerable,
            Func<IEnumerable<T>> alternativeSelector)
        {
            Guard.AgainstArgumentNull(enumerable, "enumerable");
            Guard.AgainstArgumentNull(alternativeSelector, "alternativeSelector");

            return enumerable == null || !enumerable.Any() ? alternativeSelector() : enumerable;
        }
    }
}