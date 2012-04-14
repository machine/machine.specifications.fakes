using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Fakes.Sdk;

namespace Machine.Fakes.Internal
{
    /// <summary>
    /// A set of extension methods to simplify the strong typed fake creation
    /// which is used at several places in the framework.
    /// </summary>
    static class FakeEngineExtensions
    {
        /// <summary>
        /// Creates a list of fakes.
        /// </summary>
        /// <typeparam name="T">
        /// Specifies the item type of the list. This should be an interface or an abstract class.
        /// </typeparam>
        /// <param name="amount">
        /// Specifies the amount of fakes that have to be created and inserted into the list.
        /// </param>
        /// <param name="fakeEngine">
        /// Specifies the <see cref="IFakeEngine"/> which is used to create the individual items.
        /// </param>
        /// <returns>
        /// An <see cref="IList{T}"/>.
        /// </returns>
        public static IList<T> CreateFakeCollectionOf<T>(this IFakeEngine fakeEngine, int amount)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            if (amount < 0)
            {
                throw new ArgumentOutOfRangeException("amount");
            }

            return Enumerable.Range(0, amount)
                .Select(x => (T)fakeEngine.CreateFake(typeof(T)))
                .ToList();
        }

        /// <summary>
        /// Gives strong typed access to the generic <see cref="IFakeEngine.CreateFake"/> method.
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
        public static T Stub<T>(this IFakeEngine fakeEngine, params object[] args)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            return args != null && args.Length > 0 
                ? (T) fakeEngine.CreateFake(typeof (T), args) 
                : (T) fakeEngine.CreateFake(typeof (T));
        }
    }
}