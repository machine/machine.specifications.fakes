namespace Machine.Fakes.Specs.TestClasses
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