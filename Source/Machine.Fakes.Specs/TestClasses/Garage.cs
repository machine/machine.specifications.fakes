using System.Collections;
using System.Collections.Generic;

namespace Machine.Fakes.Specs.TestClasses
{
    public class Garage : IGarage
    {
        readonly ICar _car;

        public Garage(ICar car)
        {
            _car = car;
        }

        public IEnumerator<ICar> GetEnumerator()
        {
            yield return _car;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}