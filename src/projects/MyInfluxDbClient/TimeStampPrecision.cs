using System.Collections.Generic;

namespace MyInfluxDbClient
{
    public enum TimeStampPrecision
    {
        Nanoseconds,
        Microseconds,
        Milliseconds,
        Seconds,
        Minutes,
        Hours
    }

    internal static class TimeStampPrecisionExtensions
    {
        private static readonly Dictionary<TimeStampPrecision, string> Mappings = new Dictionary<TimeStampPrecision, string>
        {
            { TimeStampPrecision.Nanoseconds, "ns"},
            { TimeStampPrecision.Microseconds, "u"},
            { TimeStampPrecision.Milliseconds, "ms"},
            { TimeStampPrecision.Seconds, "s"},
            { TimeStampPrecision.Minutes, "m"},
            { TimeStampPrecision.Hours, "h"}
        }; 

        internal static string ToUrlValue(this TimeStampPrecision value)
        {
            return Mappings[value];
        }
    }
}