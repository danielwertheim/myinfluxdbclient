using System;
using System.Globalization;

namespace InfluxDbClient
{
    public static class InfluxDbEnvironment
    {
        public static class Fields
        {
            public const string TrueString = "t";
            public const string FalseString = "f";
            public const string IntegerSuffix = "i";
        }

        public static readonly long EpochTicks = new DateTime(1970, 1, 1).Ticks;
        public static readonly string NewLine = new string(new[] { (char)10 });
        public static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
    }
}