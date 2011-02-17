using System;

namespace Machine.Fakes.Specs.Mood
{
    public class MoodIdentifier
    {
        private readonly ISystemClock _systemClock;

        public MoodIdentifier(ISystemClock systemClock)
        {
            _systemClock = systemClock;
        }

        public string IdentifyMood()
        {
            if (_systemClock.CurrentTime.DayOfWeek == DayOfWeek.Monday)
            {
                return "Pretty bad";
            }

            return "Ok";
        }
    }
}