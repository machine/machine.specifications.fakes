using System;

namespace Machine.Fakes.Specs.Mood
{
    public interface ISystemClock
    {
        DateTime CurrentTime { get; }
    }
}