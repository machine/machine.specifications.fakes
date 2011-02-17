using System.Collections.Generic;
using System.Linq;
using Machine.Fakes.Utils;
using Xunit;

namespace Machine.Fakes.Internal
{
    /// <summary>
    /// A set of extension methods to simplify the strong typed fake creation
    /// which is used at several places in the framework.
    /// </summary>
    static class FakeEngineExtensions
    {
        /// <summary>
        /// Creates a list containing 3 fake instances of the type specified 
        /// via <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the item type of the list. This should be an interface or an abstract class.
        /// </typeparam>
        /// <param name="fakeEngine">
        /// Specifies the <see cref="IFakeEngine"/> which is used to create the individual items.
        /// </param>
        /// <returns>
        /// An <see cref="IList{T}"/>.
        /// </returns>
        public static IList<T> CreateFakeCollectionOf<T>(this IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            return Enumerable.Range(0, 3)
                .Select(x => (T)fakeEngine.Stub(typeof(T)))
                .ToList();
        }

        /// <summary>
        /// Gives strong typed access to the generic <see cref="IFakeEngine.Stub"/> method.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the type to stub e.g. to create a fake for.
        /// </typeparam>
        /// <param name="fakeEngine">
        /// Specifies the <see cref="IFakeEngine"/>.
        /// </param>
        /// <returns>
        /// A new fake for the type specified via <typeparamref name="T"/>.
        /// </returns>
        public static T Stub<T>(this IFakeEngine fakeEngine)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            return (T)fakeEngine.Stub(typeof(T));
        }
    }
}