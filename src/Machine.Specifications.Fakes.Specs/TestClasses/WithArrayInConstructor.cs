namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithArrayInConstructor
    {
        public Car[] Cars { get; private set; }

        public WithArrayInConstructor(Car[] cars)
        {
            Cars = cars;
        }
    }
}
