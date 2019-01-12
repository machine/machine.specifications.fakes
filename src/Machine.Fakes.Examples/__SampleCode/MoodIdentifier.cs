using System;

namespace Machine.Fakes.Examples.SampleCode
{
    public class MoodIdentifier
    {
        readonly ISystemClock _systemClock;

        public MoodIdentifier(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }

        public string IdentifyMood()
        {
            var dayOfWeek = _systemClock.CurrentTime.DayOfWeek;

            if (dayOfWeek == DayOfWeek.Monday)
            {
                return "Pretty bad";
            }

            return "Ok";
        }
    }
}