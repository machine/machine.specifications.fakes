namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithMultipleContructorsOfSameParameterNumber
    {
        public IDriver Driver { get; private set; }
        public IGarage Garage { get; private set; }
        public ICar Car { get; private set; }

        // ReSharper disable UnusedMember.Global : constructors are used through reflection
        public WithMultipleContructorsOfSameParameterNumber(IGarage garage)
        {
            Garage = garage;
        }

        public WithMultipleContructorsOfSameParameterNumber(ICar car)
        {
            Car = car;
        }

        public WithMultipleContructorsOfSameParameterNumber(IDriver driver)
        {
            Driver = driver;
        }
    }
}
