using Machine.Specifications.Fakes.Adapters.NSubstitute;

namespace Machine.Specifications.Fakes
{
    /// <summary>
    /// Base class that adds auto mocking (grasp), I mean auto faking capabilities
    /// to Machine.Specifications. 
    /// </summary>
    /// <typeparam name="TSubject">
    /// The subject for the specification. This is the type that is created by the
    /// specification for you.
    /// </typeparam>
    public class WithSubject<TSubject> : WithSubject<TSubject, NSubstituteEngine> where TSubject : class
    {
    }
}
