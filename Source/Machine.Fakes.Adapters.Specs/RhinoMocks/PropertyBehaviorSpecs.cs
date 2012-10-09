using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Fakes.Internal;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.RhinoMocks
{
    public interface IProperties
    {
        int Prop { get; set; }
        bool TheFunction();
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Properties", "Rhinomocks")]
    public class When_setting_a_simple_property_to_value_2 : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () => _fake.Prop = 2;

        It should_has_value_2 = () => _fake.Prop.ShouldEqual(2);
        static IProperties _fake;
    }


    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Properties", "Rhinomocks")]
    public class When_setting_an_behavior_on_a_function_it_should_not_remove_property_behavior : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () =>
        {
            _fake = FakeEngineGateway.Fake<IProperties>();
        };

        Because of = () =>
        {
            _fake.WhenToldTo(f => f.TheFunction()).Return(true);
            _fake.Prop = 3;
        };

        It should_the_property_value_is_still_3 = () => _fake.Prop.ShouldEqual(3);
        static IProperties _fake;
    }

    [Subject(typeof(RhinoFakeEngine))]
    [Tags("Properties", "Rhinomocks")]
    public class When_setting_a_simple_property_to_value_2_and_then_to_3 : WithCurrentEngine<RhinoFakeEngine>
    {
        Establish context = () => _fake = FakeEngineGateway.Fake<IProperties>();

        Because of = () =>
        {
            _fake.Prop = 2;
            _fake.Prop = 3;
        };

        It should_has_value_3 = () => _fake.Prop.ShouldEqual(3);
        static IProperties _fake;
    }
}