using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public class WithCurrentEngine
    {
        Establish context = () => FakeEngineGateway.EngineIs(CreateCurrentEngine());

        private static IFakeEngine CreateCurrentEngine()
        {
            return new NSubstituteEngine();
        }
    }
}