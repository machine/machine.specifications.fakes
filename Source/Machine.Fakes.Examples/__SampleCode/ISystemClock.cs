using System;

namespace Machine.Fakes.Examples.SampleCode
{
    public interface ISystemClock
    {
        DateTime CurrentTime { get; }
    }
}