using System;
using System.ComponentModel.Design;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public class Given_a_method_was_not_configured_on_a_Fake_when_verifying_whether_it_was_accessed :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        private It should_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
                {
                    _fake = FakeEngineGateway.Fake<IServiceContainer>();
                    _fake.RemoveService(null);
                };

        private Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)));

        private It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
            {
                _fake = FakeEngineGateway.Fake<IServiceContainer>();
                _fake.RemoveService(null);
                _fake.RemoveService(null);
            };

        private Because of =
            () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        private It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }

    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_twice_but_was_only_executed_once :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
            {
                _fake = FakeEngineGateway.Fake<IServiceContainer>();
                _fake.RemoveService(null);
            };

        private Because of =
            () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).Twice());

        private It should_not_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    public class Given_a_command_was_configured_on_a_fake_when_verifying_whether_it_was_executed_only_once_but_was_excuted_twice :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
            {
                _fake = FakeEngineGateway.Fake<IServiceContainer>();
                _fake.RemoveService(null);
                _fake.RemoveService(null);
            };

        private Because of =
            () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.RemoveService(null)).OnlyOnce());

        private It should_not_throw_an_exception = () => _exception.ShouldNotBeNull();
    }

    public class Given_a_query_was_configured_on_a_fake_when_verifying_whether_it_was_executed :
        WithCurrentEngine
    {
        private static Exception _exception;
        private static IServiceContainer _fake;

        private Establish context =
            () =>
                {
                    _fake = FakeEngineGateway.Fake<IServiceContainer>();
                    _fake.GetService(null);
                };

        private Because of = () => _exception = Catch.Exception(() => _fake.WasToldTo(f => f.GetService(null)));

        private It should_not_throw_an_exception = () => _exception.ShouldBeNull();
    }
}