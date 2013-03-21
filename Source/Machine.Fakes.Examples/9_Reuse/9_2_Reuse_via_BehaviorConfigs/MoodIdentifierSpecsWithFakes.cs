using System;
using System.Linq;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.ReuseViaBehaviorConfigsWithFakes
{
    public class CurrentTime
    {
        static DateTime _currentDate;

        public CurrentTime(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        public static ISystemClock Clock;

        OnEstablish context = accessor =>
        {
            /// Can also use accessor.The<> here
            /// e.g., Clock = accessor.The<ISystemClock>();
            Clock = accessor.An<ISystemClock>();
            Clock.WhenToldTo(x => x.CurrentTime).Return(_currentDate);
        };
    }

    [Subject(typeof(MoodIdentifier)), Tags("BehaviorConfigs")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood2 : WithFakes
    {
        static MoodIdentifier _moodIdentifier;
        static string _mood;

        Establish context = () =>
        {
            With(new CurrentTime(new DateTime(2011, 02, 14)));
            _moodIdentifier = new MoodIdentifier(CurrentTime.Clock);
        };

        Because of = () =>
        {
            _mood = _moodIdentifier.IdentifyMood();
        };

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    [Subject(typeof(MoodIdentifier)), Tags("BehaviorConfigs")]
    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : WithFakes
    {
        static MoodIdentifier _moodIdentifier;
        static string _mood;

        Establish context = () =>
        {
            With(new CurrentTime(new DateTime(2011, 02, 15)));
            _moodIdentifier = new MoodIdentifier(CurrentTime.Clock);
        };

        Because of = () =>
        {
            _mood = _moodIdentifier.IdentifyMood();
        };

        It should_be_Ok = () => _mood.ShouldEqual("Ok");
    }
}
