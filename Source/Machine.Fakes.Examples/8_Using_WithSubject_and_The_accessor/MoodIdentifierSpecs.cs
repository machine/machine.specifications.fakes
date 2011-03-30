using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.UsingWithSubjectAndTheaccessor
{
    [Subject(typeof (MoodIdentifier)), Tags("Examples")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 02, 14);

            The<ISystemClock>().WhenToldTo(x => x.CurrentTime).Return(monday);
        };

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }
}