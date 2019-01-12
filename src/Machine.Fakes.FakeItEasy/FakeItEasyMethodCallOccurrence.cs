using FakeItEasy;
using FakeItEasy.Configuration;
using NumberOfTimes = FakeItEasy.Times;

namespace Machine.Fakes.Adapters.FakeItEasy
{
    class FakeItEasyMethodCallOccurrence : IMethodCallOccurrence
    {
        readonly IVoidArgumentValidationConfiguration _configuration;

        public FakeItEasyMethodCallOccurrence(IVoidArgumentValidationConfiguration configuration)
        {
            _configuration = configuration;
            _configuration.MustHaveHappened();
        }

        public void Times(int numberOfTimesTheMethodShouldHaveBeenCalled)
        {
            _configuration.MustHaveHappened(numberOfTimesTheMethodShouldHaveBeenCalled, NumberOfTimes.Exactly);
        }

        public void OnlyOnce()
        {
            _configuration.MustHaveHappenedOnceExactly();
        }

        public void Twice()
        {
            _configuration.MustHaveHappenedTwiceExactly();
        }
    }
}