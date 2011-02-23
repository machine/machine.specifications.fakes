using Machine.Fakes.Adapters.Moq;

namespace Machine.Fakes
{
    public class WithSubject<TSubject> : WithSubject<TSubject, MoqFakeEngine> where TSubject : class
    {
    }
}