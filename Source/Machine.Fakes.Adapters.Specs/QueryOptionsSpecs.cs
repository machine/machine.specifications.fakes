using System;
using System.ComponentModel.Design;
using System.Linq;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs
{
    public interface ICar
    {
        bool DoorIsOpen { get; }
    }

    public class Given_a_property_configuration_when_triggering_the_behavior : WithCurrentEngine
    {
        private static ICar _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<ICar>();

        private Because of = () => _fake.WhenToldTo(x => x.DoorIsOpen).Return(true);

        private It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.DoorIsOpen)
                      .ShouldEachConformTo(boolean => boolean);

        private It should_execute_the_configured_behavior = () => _fake.DoorIsOpen.ShouldBeTrue();
    }

    public class Given_a_simple_configured_query_when_triggering_the_behavior : WithCurrentEngine
    {
        private static IServiceContainer _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of = () => _fake.WhenToldTo(x => x.GetService(typeof (string))).Return("string");

        private It should_be_able_to_execute_the_configured_behavior_multiple_times =
            () => Enumerable
                      .Range(0, 3)
                      .Select((x, y) => _fake.GetService(typeof (string)))
                      .ShouldEachConformTo(obj => obj != null);

        private It should_execute_the_configured_behavior =
            () => _fake.GetService(typeof (string)).ShouldNotBeNull();
    }

    public class Given_an_configured_callback_on_a_query_when_triggering_the_behavior :
        WithCurrentEngine
    {
        private static IServiceContainer _fake;
        private static Type _parameter;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of =
            () => _fake
                      .WhenToldTo(x => x.GetService(typeof (string)))
                      .Return<Type>(p =>
                                        {
                                            _parameter = p;
                                            return "ReturnValue";
                                        });

        private It should_pass_the_correct_parameter_to_the_callback =
            () =>
                {
                    _fake.GetService(typeof (string));
                    _parameter.ShouldEqual(typeof (string));
                };

        private It should_return_the_return_value_of_the_callback =
            () => _fake.GetService(typeof (string)).ShouldEqual("ReturnValue");
    }

    public class Given_an_exception_configured_on_a_query_when_triggering_the_behavior :
        WithCurrentEngine
    {
        private static IServiceContainer _fake;

        private Establish context = () => _fake = FakeEngineGateway.Fake<IServiceContainer>();

        private Because of =
            () => _fake
                      .WhenToldTo(x => x.GetService(typeof (string)))
                      .Throw(new Exception("Blah"));

        private It should_execute_the_configured_behavior =
            () => Catch.Exception(() => _fake.GetService(typeof (string))).ShouldNotBeNull();
    }
}