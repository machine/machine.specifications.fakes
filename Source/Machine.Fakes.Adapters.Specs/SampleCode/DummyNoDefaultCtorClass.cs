namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public class DummyNoDefaultCtorClass
    {
        public int Value { get; private set; }

        public DummyNoDefaultCtorClass(int value)
        {
            Value = value;
        }
    }
}