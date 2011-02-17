using System;

namespace Machine.Fakes
{
    /// <summary>
    /// Represents the internal configuration. 
    /// This is accessible using the <see cref="ConfigurationAttribute"/>.
    /// </summary>
    public interface IConfiguration
    {
        /// <summary>
        ///   Specifies the fake engine to be used.
        /// </summary>
        /// <param name = "fakeEngine">
        ///   The fake engine.
        /// </param>
        /// <exception cref = "ArgumentNullException">
        ///   Thrown when <paramref name = "fakeEngine" /> is <c>null</c>.
        /// </exception>
        void FakeEngineIs(IFakeEngine fakeEngine);

        /// <summary>
        /// Specifies the fake engine type to be used for faking.
        /// This must be a type that implements <see cref="IFakeEngine"/>
        /// and has a parameterless public constructor.
        /// </summary>
        /// <param name="fakeEngineType">Type of the fake engine.</param>
        /// <exception cref="ArgumentNullException">
        /// Thrown when <paramref name="fakeEngineType"/> is <c>null</c>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// </exception>
        void FakeEngineIs(Type fakeEngineType);
    }
}