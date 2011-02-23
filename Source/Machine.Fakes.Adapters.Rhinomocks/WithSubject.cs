using Machine.Fakes.Adapters.Rhinomocks;

namespace Machine.Fakes
{
    public class WithSubject<TSubject> : WithSubject<TSubject, RhinoFakeEngine> where TSubject : class
    {
    }
}