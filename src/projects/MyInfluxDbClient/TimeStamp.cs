using System;
using MyInfluxDbClient.Protocols;

namespace MyInfluxDbClient
{
    public class TimeStamp : IEquatable<TimeStamp>
    {
        public long Ticks { get; }

        private TimeStamp(long ticks)
        {
            Ticks = ticks;
        }

        public static TimeStamp Now() => new TimeStamp(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
        public static TimeStamp From(DateTime value) => new TimeStamp(value.ToUniversalTime().Ticks - InfluxDbEnvironment.EpochTicks);

        public override bool Equals(object obj)
        {
            return Equals(obj as TimeStamp);
        }

        public bool Equals(TimeStamp other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Ticks == other.Ticks;
        }

        public override int GetHashCode()
        {
            return Ticks.GetHashCode();
        }
    }
}