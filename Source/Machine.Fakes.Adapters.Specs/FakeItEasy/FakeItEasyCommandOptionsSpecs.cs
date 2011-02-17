using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    public class Given_a_simple_configured_command : WithEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;
        static Type _recievedParameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Callback<Type>(p => _recievedParameter = p);

        It should_execute_the_configured_behavior = () =>
        {
            _fake.RemoveService(typeof(string));
            _recievedParameter.ShouldEqual(typeof (string));
        };
    }

    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior :
        WithEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () =>
        {
            var exception = Catch.Exception(() => _fake.RemoveService(typeof(string)));
            exception.ShouldNotBeNull();
        };
    }
}