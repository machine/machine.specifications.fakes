using Machine.Fakes.Adapters.FakeItEasy;

namespace Machine.Fakes
{
    public class WithSubject<TSubject> : WithSubject<TSubject, FakeItEasyEngine> where TSubject : class
    {
    }
}