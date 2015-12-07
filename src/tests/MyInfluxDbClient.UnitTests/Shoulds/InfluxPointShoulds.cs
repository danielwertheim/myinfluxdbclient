using System.Collections.Generic;
using FluentAssertions;

namespace MyInfluxDbClient.UnitTests.Shoulds
{
    internal static class InfluxPointShoulds
    {
        internal static void ShouldContainTag(this InfluxPoint point, string key, string value)
        {
            point.Tags.Should().Contain(new KeyValuePair<string, string>(key, value));
        }

        internal static void ShouldContainField(this InfluxPoint point, string key, string value)
        {
            point.Fields.Should().Contain(new KeyValuePair<string, string>(key, $"\"{value}\""));
        }
    }
}