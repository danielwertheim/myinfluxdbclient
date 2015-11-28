namespace MyInfluxDbClient
{
    public static class TimeStampResolutions
    {
        public static readonly TimeStampResolution Default = new NanosecondsResolution();
        public static readonly TimeStampResolution Nanoseconds = Default;
        public static readonly TimeStampResolution Microseconds = new MicrosecondsResolution();
        public static readonly TimeStampResolution Milliseconds = new MillisecondsResolution();
        public static readonly TimeStampResolution Seconds = new SecondsResolution();
        public static readonly TimeStampResolution Minutes = new MinuteResolution();
        public static readonly TimeStampResolution Hours = new HourResolution();
    }
}