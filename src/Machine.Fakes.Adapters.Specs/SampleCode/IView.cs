namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public interface IView
    {
        bool TryLogin(string user, string password);

        bool AreBothOdd(int? one, int? two);
    }
}