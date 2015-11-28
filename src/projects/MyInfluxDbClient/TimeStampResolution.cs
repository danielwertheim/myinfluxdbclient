using System;

namespace MyInfluxDbClient
{
    public abstract class TimeStampResolution
    {
        protected const long NanoSecondsPerTick = 100;
        protected const long TicksPerMicrosecond = 10;
        protected const long TicksPerMillisecond = TimeSpan.TicksPerMillisecond;
        protected const long TicksPerSecond = TimeSpan.TicksPerSecond;
        protected const long TicksPerMinute = TimeSpan.TicksPerMinute;
        protected const long TicksPerHour = TimeSpan.TicksPerHour;

        public long NanosecondsFrom(TimeStamp timeStamp)
        {
            return OnApply(timeStamp) * NanoSecondsPerTick;
        }

        protected abstract long OnApply(TimeStamp timeStamp);
    }
}