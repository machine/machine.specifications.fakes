






using System;
using System.Linq;
using Machine.Fakes.Adapters.Specs.SampleCode;
using Machine.Fakes.Internal;
using Machine.Specifications;


#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.Rhinomocks
{
	using Machine.Fakes.Adapters.Rhinomocks;

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_property_configuration_when_triggering_the_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        static ICar _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ICar>();

        Because of = () => _fake.WhenToldTo(x => x.DoorIsOpen).Return(true);

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.DoorIsOpen)
                      .ShouldEachConformTo(boolean => boolean);

        It should_execute_the_configured_behavior = () => _fake.DoorIsOpen.ShouldBeTrue();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.GetService(typeof(string))).Return("string");

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof(string)))
                      .ShouldEachConformTo(obj => obj != null);

        It should_execute_the_configured_behavior = () => _fake.GetService(typeof(string)).ShouldNotBeNull();
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        static IServiceContainer _fake;
        static Type _parameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Return<Type>(p =>
                               {
                                   _parameter = p;
                                   return "ReturnValue";
                               });

        It should_pass_the_correct_parameter_to_the_callback = () =>
        {
            _fake.GetService(typeof(string));
            _parameter.ShouldEqual(typeof(string));
        };

        It should_return_the_return_value_of_the_callback = () => _fake.GetService(typeof(string)).ShouldEqual("ReturnValue");
    }

    [Subject(typeof(RhinoFakeEngine))]
    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.GetService(typeof(string))).ShouldNotBeNull();
    }
}

#endif




#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.NSubstitute
{
	using Machine.Fakes.Adapters.NSubstitute;

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_property_configuration_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static ICar _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ICar>();

        Because of = () => _fake.WhenToldTo(x => x.DoorIsOpen).Return(true);

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.DoorIsOpen)
                      .ShouldEachConformTo(boolean => boolean);

        It should_execute_the_configured_behavior = () => _fake.DoorIsOpen.ShouldBeTrue();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.GetService(typeof(string))).Return("string");

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof(string)))
                      .ShouldEachConformTo(obj => obj != null);

        It should_execute_the_configured_behavior = () => _fake.GetService(typeof(string)).ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;
        static Type _parameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Return<Type>(p =>
                               {
                                   _parameter = p;
                                   return "ReturnValue";
                               });

        It should_pass_the_correct_parameter_to_the_callback = () =>
        {
            _fake.GetService(typeof(string));
            _parameter.ShouldEqual(typeof(string));
        };

        It should_return_the_return_value_of_the_callback = () => _fake.GetService(typeof(string)).ShouldEqual("ReturnValue");
    }

    [Subject(typeof(NSubstituteEngine))]
    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.GetService(typeof(string))).ShouldNotBeNull();
    }
}

#endif







namespace Machine.Fakes.Adapters.Specs.Moq
{
	using Machine.Fakes.Adapters.Moq;

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_property_configuration_when_triggering_the_behavior : WithCurrentEngine<MoqFakeEngine>
    {
        static ICar _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ICar>();

        Because of = () => _fake.WhenToldTo(x => x.DoorIsOpen).Return(true);

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.DoorIsOpen)
                      .ShouldEachConformTo(boolean => boolean);

        It should_execute_the_configured_behavior = () => _fake.DoorIsOpen.ShouldBeTrue();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.GetService(typeof(string))).Return("string");

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof(string)))
                      .ShouldEachConformTo(obj => obj != null);

        It should_execute_the_configured_behavior = () => _fake.GetService(typeof(string)).ShouldNotBeNull();
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;
        static Type _parameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Return<Type>(p =>
                               {
                                   _parameter = p;
                                   return "ReturnValue";
                               });

        It should_pass_the_correct_parameter_to_the_callback = () =>
        {
            _fake.GetService(typeof(string));
            _parameter.ShouldEqual(typeof(string));
        };

        It should_return_the_return_value_of_the_callback = () => _fake.GetService(typeof(string)).ShouldEqual("ReturnValue");
    }

    [Subject(typeof(MoqFakeEngine))]
    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior : WithCurrentEngine<MoqFakeEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.GetService(typeof(string))).ShouldNotBeNull();
    }
}





#if !NETSTANDARD



namespace Machine.Fakes.Adapters.Specs.FakeItEasy
{
	using Machine.Fakes.Adapters.FakeItEasy;

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_property_configuration_when_triggering_the_behavior : WithCurrentEngine<FakeItEasyEngine>
    {
        static ICar _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<ICar>();

        Because of = () => _fake.WhenToldTo(x => x.DoorIsOpen).Return(true);

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.DoorIsOpen)
                      .ShouldEachConformTo(boolean => boolean);

        It should_execute_the_configured_behavior = () => _fake.DoorIsOpen.ShouldBeTrue();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.GetService(typeof(string))).Return("string");

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof(string)))
                      .ShouldEachConformTo(obj => obj != null);

        It should_execute_the_configured_behavior = () => _fake.GetService(typeof(string)).ShouldNotBeNull();
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;
        static Type _parameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Return<Type>(p =>
                               {
                                   _parameter = p;
                                   return "ReturnValue";
                               });

        It should_pass_the_correct_parameter_to_the_callback = () =>
        {
            _fake.GetService(typeof(string));
            _parameter.ShouldEqual(typeof(string));
        };

        It should_return_the_return_value_of_the_callback = () => _fake.GetService(typeof(string)).ShouldEqual("ReturnValue");
    }

    [Subject(typeof(FakeItEasyEngine))]
    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior : WithCurrentEngine<FakeItEasyEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof(string)))
                               .Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.GetService(typeof(string))).ShouldNotBeNull();
    }
}

#endif


