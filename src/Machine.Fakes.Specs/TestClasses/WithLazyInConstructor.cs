using System;

namespace Machine.Fakes.Specs.TestClasses
{
    public class WithLazyInConstructor
    {
        readonly Lazy<ICar> lazyCar;

        public ICar Car
        {
            get
            {
                return lazyCar.Value;
            }
        }

        public WithLazyInConstructor(Lazy<ICar> lazyCar)
        {
            this.lazyCar = lazyCar;
        }
    }
}