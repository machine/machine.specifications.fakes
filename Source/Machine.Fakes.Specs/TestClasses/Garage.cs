namespace Machine.Fakes.Specs.TestClasses
{
    public class Garage
    {
        ICar _car;

        public Garage(ICar car)
        {
            _car = car;
        }
    }
}