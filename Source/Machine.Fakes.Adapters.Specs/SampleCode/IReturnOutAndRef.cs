namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public interface IReturnOutAndRef
    {
        void GetTwoValues(string input, out string output, ref object additional);
    }
}