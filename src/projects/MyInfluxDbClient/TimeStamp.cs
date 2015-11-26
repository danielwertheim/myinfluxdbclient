using System;

namespace MyInfluxDbClient
{
    public struct TimeStamp
    {
        public long Ticks { get; }

        private TimeStamp(long ticks)
        {
            Ticks = ticks;
        }

        public static TimeStamp Now() => new TimeStamp(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
        public static TimeStamp From(DateTime value) => new TimeStamp(value.ToUniversalTime().Ticks - InfluxDbEnvironment.EpochTicks);
    }
}