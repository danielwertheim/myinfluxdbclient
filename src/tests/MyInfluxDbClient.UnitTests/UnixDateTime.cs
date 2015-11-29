using System;
using MyInfluxDbClient.Protocols;

namespace MyInfluxDbClient.UnitTests
{
    internal static class UnixDateTime
    {
        internal static DateTime Now()
        {
            return new DateTime(DateTime.UtcNow.Ticks - LineProtocolFormat.EpochTicks);
        }
    }
}