using System;

namespace MyInfluxDbClient
{
    public static class InfluxDbEnvironment
    {
        public static readonly long EpochTicks = new DateTime(1970, 1, 1).Ticks;
    }
}