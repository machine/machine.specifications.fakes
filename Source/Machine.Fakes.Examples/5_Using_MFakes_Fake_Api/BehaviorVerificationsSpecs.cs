using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.UsingMFakesApi
{
    [Subject(typeof(FakeApi)), Tags("Examples")]
    public class When_a_method_is_called_on_a_fake : WithFakes
    {
        static IServiceContainer _subject;

        Establish context = () =>
        {
            _subject = An<IServiceContainer>();
        };

        Because of = () => _subject.GetService(null);

        It should_be_able_to_verify_that = () => _subject.WasToldTo(s => s.GetService(null));
    }
}