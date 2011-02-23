using System;
using Machine.Specifications;

namespace Machine.Fakes.Specs.Mood.BehaviorConfigs
{
    public class CurrentTime : BehaviorConfigBase
    {
        public DateTime Time { get; set; }

        public CurrentTime(DateTime time)
        {
            Time = time;
        }

        public override void EstablishContext(IFakeAccessor fakeAccessor)
        {
            fakeAccessor.The<ISystemClock>()
                .WhenToldTo(x => x.CurrentTime)
                .Return(Time);
        }
    }

    public class Given_the_current_day_is_monday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => With(new CurrentTime(new DateTime(2011, 2, 14)));

        Because of = () => _mood = Subject.IdentifyMood();

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => With(new CurrentTime(new DateTime(2011, 2, 15)));

        Because of = () => _mood = Subject.IdentifyMood();

        It should_be_Ok = () => _mood.ShouldEqual("Ok");
    }
}