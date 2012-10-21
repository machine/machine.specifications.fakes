namespace Machine.Fakes.Specs.TestClasses
{
    public class WithBoolInConstructor
    {
        public bool Yes { get; set; }

        public WithBoolInConstructor(bool yes)
        {
            Yes = yes;
        }
    }
}