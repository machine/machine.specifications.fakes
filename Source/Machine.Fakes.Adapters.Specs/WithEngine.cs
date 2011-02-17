using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public class WithEngine<TEngine> where TEngine: IFakeEngine, new()
    {
        Establish context = () =>
        {
            var engine = new TEngine();

            FakeEngineGateway.EngineIs(engine);
        };
    }
}