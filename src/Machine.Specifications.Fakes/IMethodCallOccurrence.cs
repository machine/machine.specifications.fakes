namespace Machine.Specifications.Fakes
{
    /// <summary>
    /// Interface for detail configuration used by <see cref="FakeApi.WasToldTo{TFake}"/>.
    /// </summary>
    public interface IMethodCallOccurrence
    {
        /// <summary>
        ///   Specifies that the behavior on the fake should be executed several times. <paramref name = "numberOfTimesTheMethodShouldHaveBeenCalled" />
        ///   specifies exactly how often.
        /// </summary>
        /// <param name = "numberOfTimesTheMethodShouldHaveBeenCalled">
        ///   The number of times the behavior should have been executed.
        /// </param>
        void Times(int numberOfTimesTheMethodShouldHaveBeenCalled);

        /// <summary>
        ///   Specifies that the behavior on the fake should only be executed once.
        /// </summary>
        void OnlyOnce();

        /// <summary>
        ///   Specifies that the behavior on the fake should be called twice.
        /// </summary>
        void Twice();
    }
}
