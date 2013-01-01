namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public abstract class ClassWithUnfakableParameter
    {
        public string ReceivedConstructorArgument { get; private set; }

        protected ClassWithUnfakableParameter(string inner)
        {
            ReceivedConstructorArgument = inner;
        }

        public virtual string VirtualMethod()
        {
            return ReceivedConstructorArgument;
        }
    }
}