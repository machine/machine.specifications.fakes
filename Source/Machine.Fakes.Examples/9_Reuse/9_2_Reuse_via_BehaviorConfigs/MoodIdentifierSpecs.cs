using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.ReuseViaBehaviorConfigs
{
    public class CurrentTime 
    {
        static  DateTime _currentDate;

        public CurrentTime(DateTime currentDate)
        {
            _currentDate = currentDate;
        }

        OnEstablish context = accessor =>
        {
            accessor.The<ISystemClock>().WhenToldTo(x => x.CurrentTime).Return(_currentDate);
        };
    }

    [Subject(typeof (MoodIdentifier)), Tags("BehaviorConfigs")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => With(new CurrentTime(new DateTime(2011, 02, 14)));

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    [Subject(typeof (MoodIdentifier)), Tags("BehaviorConfigs")]
    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => With(new CurrentTime(new DateTime(2011, 02, 15)));

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };

        It should_be_Ok = () => _mood.ShouldEqual("Ok");
    }
}