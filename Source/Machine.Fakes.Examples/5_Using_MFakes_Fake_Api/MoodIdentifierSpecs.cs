using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.UsingMFakesApi
{
    [Subject(typeof(MoodIdentifier)), Tags("Examples")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithFakes
    {
        static MoodIdentifier _moodIdentifier;
        static DateTime _monday;
        static string _mood;

        Establish context = () =>
        {
            _monday = new DateTime(2011, 2, 14);

            var clock = An<ISystemClock>();

            clock.WhenToldTo(x => x.CurrentTime).Return(_monday);

            _moodIdentifier = new MoodIdentifier(clock);
        };

        Because of = () =>
        {
            _mood = _moodIdentifier.IdentifyMood();
        };

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    [Subject(typeof(MoodIdentifier)), Tags("Examples")]
    public class The_mood_on_a_tuesday : WithFakes
    {
        static MoodIdentifier _moodIdentifier;
        static DateTime _tuesday;
        static string _mood;

        Establish context = () =>
        {
            _tuesday = new DateTime(2011, 2, 15);

            var clock = An<ISystemClock>();

            clock.WhenToldTo(x => x.CurrentTime).Return(_tuesday);

            _moodIdentifier = new MoodIdentifier(clock);
        };

        Because of = () =>
        {
            _mood = _moodIdentifier.IdentifyMood();
        };

        It should_be_ok = () => _mood.ShouldEqual("Ok");
    }
}