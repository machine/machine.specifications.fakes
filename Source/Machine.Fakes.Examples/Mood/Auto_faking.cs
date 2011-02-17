using System;
using Machine.Specifications;

namespace Machine.Fakes.Specs.Mood.AutoFaking
{
    public class Given_the_current_day_is_monday_when_identifying_my_mood : auto_fake<MoodIdentifier>
    {
        static string _mood;

        Establish context = () =>
        {
            var monday = new DateTime(2011, 2, 14);

            The<ISystemClock>()
                .WhenToldTo(x => x.CurrentTime)
                .Return(monday);
        };

        Because of = () => _mood = Subject.IdentifyMood();

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : auto_fake<MoodIdentifier>
    {
        static string _mood;

        Establish context = () =>
        {
            var tuesday = new DateTime(2011, 2, 15);

            The<ISystemClock>()
                .WhenToldTo(x => x.CurrentTime)
                .Return(tuesday);
        };

        Because of = () => _mood = Subject.IdentifyMood();

        It should_be_Ok = () => _mood.ShouldEqual("Ok");
    }
}