using Machine.Fakes.Sdk;

namespace Machine.Fakes.Internal
{
    /// <summary>
    /// A set of extension methods to simplify the strong typed fake creation
    /// which is used at several places in the framework.
    /// </summary>
    static class FakeEngineExtensions
    {
        /// <summary>Gives strong typed access to the generic <see cref="IFakeEngine.CreateFake"/> method.</summary>
        /// <typeparam name="T">Specifies the type to stub e.g. to create a fake for.</typeparam>
        /// <param name="fakeEngine">Specifies the <see cref="IFakeEngine"/>.</param>
        /// <param name="args">Constructor arguments for fake to create.</param>
        /// <returns>A new fake for the type specified via <typeparamref name="T"/>.</returns>
        public static T Stub<T>(this IFakeEngine fakeEngine, params object[] args)
        {
            Guard.AgainstArgumentNull(fakeEngine, "fakeEngine");

            return args != null && args.Length > 0
                ? (T)fakeEngine.CreateFake(typeof(T), args)
                : (T)fakeEngine.CreateFake(typeof(T));
        }
    }
}