using Machine.Fakes.Adapters.NSubstitute;

namespace Machine.Fakes
{
    public class WithSubject<TSubject> : WithSubject<TSubject, NSubstituteEngine> where TSubject : class
    {
    }
}