using System;
using FakeItEasy;
using FakeItEasy.Configuration;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    class FakeItEasyMethodCallOccurance : IMethodCallOccurance
    {
        readonly IVoidArgumentValidationConfiguration _configuration;

        public FakeItEasyMethodCallOccurance(IVoidArgumentValidationConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.MustHaveHappened();
        }

        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            _configuration.MustHaveHappened(Repeated.Exactly.Times(numberOfTimesTheMethodShouldHaveBeenCalled));
        }

        public void OnlyOnce()
        {
            _configuration.MustHaveHappened(Repeated.Exactly.Once);
        }

        public void Twice()
        {
            _configuration.MustHaveHappened(Repeated.Exactly.Twice);
        }
    }
}