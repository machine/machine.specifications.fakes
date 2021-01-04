namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public interface ILookupService
    {
        void TryLookup(string key, out string value); 
    }
}