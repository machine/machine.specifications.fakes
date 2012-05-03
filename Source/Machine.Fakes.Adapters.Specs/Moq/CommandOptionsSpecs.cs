using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.Moq;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.Moq
{
    [Subject(typeof(MoqFakeEngine))]
    [Tags("CommandOptions", "Moq")]
    public class Given_a_simple_configured_command : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;
        static Type _receivedParameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string)))
                               .Callback<Type>(p => _receivedParameter = p);

        It should_execute_the_configured_behavior = () =>
        {
            _fake.RemoveService(typeof(string));
            _receivedParameter.ShouldEqual(typeof(string));
        };
    }

    [Subject(typeof(MoqFakeEngine))]
    [Tags("CommandOptions", "Moq")]
    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior :
        WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.RemoveService(typeof(string))).ShouldNotBeNull();
    }
}