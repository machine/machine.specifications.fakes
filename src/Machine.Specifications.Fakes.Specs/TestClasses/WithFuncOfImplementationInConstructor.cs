using System;

namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithFuncOfImplementationInConstructor
    {
        readonly Func<Car> carFactory;

        public Car Car { get { return carFactory(); } }

        public WithFuncOfImplementationInConstructor(Func<Car> carFactory)
        {
            this.carFactory = carFactory;
        }
    }
}
