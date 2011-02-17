using System;
using Machine.Specifications;

namespace Machine.Fakes.Specs.Mood.Faking
{
    public class Given_the_current_day_is_monday_when_identifying_my_mood : with_fakes
    {
        static MoodIdentifier _subject;
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 2, 14);
            var systemClock = An<ISystemClock>();
            
            systemClock
                .WhenToldTo(x => x.CurrentTime)
                .Return(monday);

            _subject = new MoodIdentifier(systemClock);
        };

        Because of = () => _mood = _subject.IdentifyMood();

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : auto_fake<MoodIdentifier>
    {
        static MoodIdentifier _subject;
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 2, 15);
            var systemClock = An<ISystemClock>();

            systemClock
                .WhenToldTo(x => x.CurrentTime)
                .Return(monday);

            _subject = new MoodIdentifier(systemClock);
        };

        Because of = () => _mood = _subject.IdentifyMood();

        It should_be_pretty_ok = () => _mood.ShouldEqual("Ok");
    }

}
