using System;
using System.ComponentModel.Design;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public class AfterInitializingANewFakeCurrentEngine : WithCurrentEngine
    {
        private static IServiceContainer _fake;

        private Because of = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private It should_be_able_to_create_an_instance = () => _fake.ShouldNotBeNull();
    }

    public class Given_that_a_call_was_not_expected_to_happen_but_happened_when_verifying : WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
                {
                    _fake = FakeEngineGateway.Fake<IServiceContainer>();
                    _fake.RemoveService(null);
                };

        private Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        private It should_have_thrown_an_exception = () => _exception.ShouldNotBeNull();
    }

    public class Given_that_a_call_was_not_expected_to_happen_and_did_not_happened_when_verifying :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of = () => _exception = Catch.Exception(() => _fake.WasNotToldTo(f => f.RemoveService(null)));

        private It should_not_have_thrown_an_exception = () => _exception.ShouldBeNull();
    }
}