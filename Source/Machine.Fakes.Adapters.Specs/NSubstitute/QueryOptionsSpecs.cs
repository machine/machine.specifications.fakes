using System;
using System.ComponentModel.Design;
using System.Linq;
using Machine.Fakes.Adapters.NSubstitute;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.NSubstitute
{
    public interface ICar
    {
        bool DoorIsOpen { get; }
    }

    [Subject(typeof(NSubstituteEngine))]
    [Tags("QueryOptions", "NSubstitute")]
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
    [Tags("QueryOptions", "NSubstitute")]
    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake.WhenToldTo(x => x.GetService(typeof (string))).Return("string");

        It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof (string)))
                      .ShouldEachConformTo(obj => obj != null);

        It should_execute_the_configured_behavior = () => _fake.GetService(typeof (string)).ShouldNotBeNull();
    }

    [Subject(typeof(NSubstituteEngine))]
    [Tags("QueryOptions", "NSubstitute")]
    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior :
        WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;
        static Type _parameter;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof (string)))
                               .Return<Type>(p =>
                               {
                                   _parameter = p;
                                   return "ReturnValue";
                               });

        It should_pass_the_correct_parameter_to_the_callback = () =>
        {
            _fake.GetService(typeof (string));
            _parameter.ShouldEqual(typeof (string));
        };

        It should_return_the_return_value_of_the_callback = () => _fake.GetService(typeof (string)).ShouldEqual("ReturnValue");
    }

    [Subject(typeof(NSubstituteEngine))]
    [Tags("QueryOptions", "NSubstitute")]
    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior :
        WithCurrentEngine<NSubstituteEngine>
    {
        static IServiceContainer _fake;

        Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        Because of = () => _fake
                               .WhenToldTo(x => x.GetService(typeof (string)))
                               .Throw(new Exception("Blah"));

        It should_execute_the_configured_behavior = () => Catch.Exception(() => _fake.GetService(typeof (string))).ShouldNotBeNull();
    }
}