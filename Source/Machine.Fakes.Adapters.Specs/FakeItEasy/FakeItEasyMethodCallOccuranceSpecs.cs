using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed : WithEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.RemoveService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed : WithEngine<FakeItEasyEngine>
    {
        static Exception _exception;
        static IServiceContainer _fake;

        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IServiceContainer>();
            _fake.GetService(null);
        };

        Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}