using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class TimeStampTests : UnitTests
    {
        [Test]
        public void Now_Should_give_TimeStamp_with_UTC_ticks_from_epoch()
        {
            var expexted = GetUnixDateTime();

            var ts = TimeStamp.Now();

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void From_Given_a_non_UTC_date_time_Should_give_TimeStamp_with_UTC_ticks_from_epoch()
        {
            var expexted = GetUnixDateTime();

            var ts = TimeStamp.From(DateTime.Now);

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void From_Given_a_UTC_date_time_Should_give_TimeStamp_with_UTC_ticks_from_epoch()
        {
            var expexted = GetUnixDateTime();

            var ts = TimeStamp.From(DateTime.UtcNow);

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void Equals_Given_based_on_different_date_time_values_Should_return_false()
        {
            var now = DateTime.Now;
            var tsX = TimeStamp.From(now);
            var tsY = TimeStamp.From(now.AddTicks(-1));

            tsX.Equals(tsY).Should().BeFalse();
        }

        [Test]
        public void Equals_Given_based_on_same_date_time_values_Should_return_true()
        {
            var now = DateTime.Now;
            var tsX = TimeStamp.From(now);
            var tsY = TimeStamp.From(now);

            tsX.Equals(tsY).Should().BeTrue();
        }

        private static DateTime GetUnixDateTime()
        {
            return new DateTime(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
        }
    }
}