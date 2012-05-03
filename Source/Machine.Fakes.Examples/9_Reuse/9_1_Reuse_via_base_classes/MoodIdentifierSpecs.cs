using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.ReuseViaBaseClasses
{
    public abstract class TimeSpecification<T> : WithSubject<T> where T : class
    {
        protected static void CurrentTimeIs(DateTime time)
        {
            The<ISystemClock>().WhenToldTo(x => x.CurrentTime).Return(time);
        }
    }

    [Subject(typeof(MoodIdentifier)), Tags("Examples")]
    public class Given_the_current_day_is_monday_when_identifying_my_mood : TimeSpecification<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => CurrentTimeIs(new DateTime(2011, 02, 14));

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };

        It should_be_pretty_bad = () => _mood.ShouldEqual("Pretty bad");
    }

    [Subject(typeof(MoodIdentifier)), Tags("Examples")]
    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : TimeSpecification<MoodIdentifier>
    {
        static string _mood;

        Establish context = () => CurrentTimeIs(new DateTime(2011, 02, 15));

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };

        It should_be_Ok = () => _mood.ShouldEqual("Ok");
    }
}