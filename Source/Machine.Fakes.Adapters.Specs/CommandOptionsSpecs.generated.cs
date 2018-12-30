






using System;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;



#if !NETSTANDARD


namespace Machine.Fakes.Adapters.Specs.Rhinomocks
{

	using Machine.Fakes.Adapters.Rhinomocks;

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

#endif





#if !NETSTANDARD


namespace Machine.Fakes.Adapters.Specs.NSubstitute
{

	using Machine.Fakes.Adapters.NSubstitute;

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

#endif







namespace Machine.Fakes.Adapters.Specs.Moq
{

	using Machine.Fakes.Adapters.Moq;

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






#if !NETSTANDARD


namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{

	using Machine.Fakes.Adapters.FakeItEasy;

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

#endif


