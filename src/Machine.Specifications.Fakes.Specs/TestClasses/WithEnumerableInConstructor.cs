using System.Collections.Generic;

namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithEnumerableInConstructor
    {
        public IEnumerable<Car> Cars { get; set; }

        public WithEnumerableInConstructor(IEnumerable<Car> cars)
        {
            Cars = cars;
        }
    }
}
