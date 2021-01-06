namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithInterfaceArrayInConstructor
    {
        public ICar[] Cars { get; set; }

        public WithInterfaceArrayInConstructor(ICar[] cars)
        {
            Cars = cars;
        }
    }
}
