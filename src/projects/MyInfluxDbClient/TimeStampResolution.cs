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

        public static readonly TimeStampResolution Nanoseconds = new NanosecondsResolution();
        public static readonly TimeStampResolution Microseconds = new MicrosecondsResolution();
        public static readonly TimeStampResolution Milliseconds = new MillisecondsResolution();
        public static readonly TimeStampResolution Seconds = new SecondsResolution();
        public static readonly TimeStampResolution Minutes = new MinuteResolution();
        public static readonly TimeStampResolution Hours = new HourResolution();
    }
}