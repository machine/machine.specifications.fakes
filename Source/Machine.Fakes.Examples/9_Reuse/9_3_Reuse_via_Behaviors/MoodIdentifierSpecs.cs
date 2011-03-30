using System;
using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.ReuseViaBehaviors
{
    [Behaviors]
    public class Any_day_except_monday
    {
        protected static string _mood;

        It should_be_OK = () => _mood.ShouldEqual("Ok");
    }

    [Subject(typeof (MoodIdentifier)), Tags("Behavior")]
    public class Given_the_current_day_is_tuesday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        protected static string _mood;
        Behaves_like<Any_day_except_monday> any_day_except_monday;

        Establish context = () =>
        {
            var tuesday = new DateTime(2011, 02, 15);

            The<ISystemClock>().WhenToldTo(x => x.CurrentTime).Return(tuesday);
        };

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };
    }

    [Subject(typeof (MoodIdentifier)), Tags("Behavior")]
    public class Given_the_current_day_is_wednesday_when_identifying_my_mood : WithSubject<MoodIdentifier>
    {
        protected static string _mood;
        Behaves_like<Any_day_except_monday> any_day_except_monday;

        Establish context = () =>
        {
            var wednesday = new DateTime(2011, 02, 16);

            The<ISystemClock>().WhenToldTo(x => x.CurrentTime).Return(wednesday);
        };

        Because of = () =>
        {
            _mood = Subject.IdentifyMood();
        };
    }
}