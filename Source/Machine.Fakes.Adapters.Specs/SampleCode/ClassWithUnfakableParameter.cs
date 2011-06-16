namespace Machine.Fakes.Adapters.Specs.SampleCode
{
    public abstract class ClassWithUnfakableParameter
    {
        public string RecievedCtorArgument { get; private set; }

        protected ClassWithUnfakableParameter(string inner)
        {
            RecievedCtorArgument = inner;
        }

        public virtual string VirtualMethod()
        {
            return RecievedCtorArgument;
        }
    }
}