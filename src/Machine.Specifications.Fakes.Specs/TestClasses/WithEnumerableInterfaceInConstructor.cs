using System.Collections.Generic;

namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithEnumerableInterfaceInConstructor
    {
        public IEnumerable<ICar> Cars { get; set; }

        public WithEnumerableInterfaceInConstructor(IEnumerable<ICar> cars)
        {
            Cars = cars;
        }
    }
}
