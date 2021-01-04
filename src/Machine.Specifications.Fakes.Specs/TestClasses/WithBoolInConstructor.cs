namespace Machine.Fakes.Specs.TestClasses
{
    public class WithBoolInConstructor
    {
        public bool Yes { get; private set; }

        public WithBoolInConstructor(bool yes)
        {
            Yes = yes;
        }
    }
}