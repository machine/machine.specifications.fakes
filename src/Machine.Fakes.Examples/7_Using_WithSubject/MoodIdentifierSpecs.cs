using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.UsingWithSubject
{
    [Subject(typeof(MoodIdentifier)), Tags("Examples")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        Establish context = () =>
        {
        };

        Because of = () =>
        {
            var subject = Subject;
        };

        It should_be_pretty_bad = () =>
        {
        };
    }
}