using System;

using Machine.Fakes.Examples.SampleCode;
using Machine.Specifications;

namespace Machine.Fakes.Examples.ReuseViaWithFakesHelperClass
{
    // Although this sort of usage of WithFakes is possible,
    // it is recommended to use BehaviorConfigs instead
    public class ClockFaker : WithFakes
    {
        public ISystemClock CreateClockFake(DateTime currentDate)
        {
            var result = An<ISystemClock>();
            result.WhenToldTo(x => x.CurrentTime).Return(currentDate);
            return result;
        }
    }

    [Subject(typeof(MoodIdentifier))]
    public class Given_the_current_day_is_monday_when_identifying_my_mood
    {
        static string mood;
        static MoodIdentifier subject;

        Establish context = () => subject = new MoodIdentifier(
            new ClockFaker().CreateClockFake(new DateTime(2011, 02, 14)));

        Because of = () => mood = subject.IdentifyMood();

        It should_be_pretty_bad = () => mood.ShouldEqual("Pretty bad");
    }
}