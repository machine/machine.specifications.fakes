using System;
using System.ComponentModel.Design;
using Machine.Fakes.Adapters.FakeItEasy;
using Machine.Fakes.Adapters.Moq;
using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RhinoMocks
{
    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_simple_configured_command : WithCurrentEngine<RhinoFakeEngine>
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

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.RemoveService(typeof(string))).ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.NSubstitute
{
    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_simple_configured_command : WithCurrentEngine<NSubstituteEngine>
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

    [Subject(typeof(NSubstituteEngine))]
    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.RemoveService(typeof(string))).ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.Moq
{
    [Subject(typeof(MoqFakeEngine))]
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
    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.RemoveService(typeof(string))).ShouldNotBeNull();
    }
}

namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_simple_configured_command : WithCurrentEngine<FakeItEasyEngine>
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

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_an_exception_configured_on_a_command_when_triggering_the_behavior : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.RemoveService(typeof(string))).Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.RemoveService(typeof(string))).ShouldNotBeNull();
    }
}
