using Machine.Fakes.Adapters.Rhinomocks;
using Machine.Specifications;

namespace Machine.Fakes.Adapters.Specs.Bugs
{
    public class When_using_rhino_fake_engine_to_mock_a_property_with_get_and_set : WithFakes<RhinoFakeEngine>
    {
        Establish context = () =>
        {
            _subject = An<IFoo>();
        };

        Because of = () => _subject.WhenToldTo(a => a.Value).Return("abc");

        It should_return_the_mocked_value = () => _subject.Value.ShouldEqual("abc");

        static IFoo _subject;
    }

    public interface IFoo
    {
        string Value { get; set; }
    }
}