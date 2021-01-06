using System;

namespace Machine.Specifications.Fakes.Specs.TestClasses
{
    public class WithFuncOfInterfaceInConstructor
    {
        readonly Func<ICar> carFactory;

        public ICar Car
        {
            get
            {
                return carFactory();
            }
        }

        public WithFuncOfInterfaceInConstructor(Func<ICar> carFactory)
        {
            this.carFactory = carFactory;
        }
    }
}
