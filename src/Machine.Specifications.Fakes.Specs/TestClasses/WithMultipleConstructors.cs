namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithMultipleConstructors
    {
        public ICar Car { get; set; }

        public WithMultipleConstructors()
        {
        }

        public WithMultipleConstructors(ICar car)
        {
            Car = car;
        }
    }
}
