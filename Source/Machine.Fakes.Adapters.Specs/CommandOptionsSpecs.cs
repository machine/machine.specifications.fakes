using System;
using System.ComponentModel.Design;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public class Given_a_simple_configured_command : WithCurrentEngine
    {
        private static IServiceContainer _fake;
        private static Type _recievedParameter;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of =
            () => _fake
                      .WhenToldTo(x => x.RemoveService(typeof (string)))
                      .Callback<Type>(p => _recievedParameter = p);

        private It should_execute_the_configured_behavior =
            () =>
                {
                    _fake.RemoveService(typeof (string));
                    _recievedParameter.ShouldEqual(typeof (string));
                };
    }

    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior :
        WithCurrentEngine
    {
        private static IServiceContainer _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of =
            () => _fake
                      .WhenToldTo(x => x.RemoveService(typeof (string)))
                      .Throw(new Exception("Blah"));

        private It should_execute_the_configured_behavior =
            () => Catch.Exception(() => _fake.RemoveService(typeof (string))).ShouldNotBeNull();
    }
}