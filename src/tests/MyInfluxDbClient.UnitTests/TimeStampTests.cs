using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class TimeStampTests : UnitTests
    {
        [Test]
        public void Now_Should_return_TimeStamp_with_UTC_ticks_from_epoch()
        {
            var expexted = UnixDateTime.Now();

            var ts = TimeStamp.Now();

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void From_Should_give_TimeStamp_with_UTC_ticks_from_epoch_When_a_non_UTC_date_time_is_passed()
        {
            var expexted = UnixDateTime.Now();

            var ts = TimeStamp.From(DateTime.Now);

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void From_Should_give_TimeStamp_with_UTC_ticks_from_epoch_When_an_UTC_date_time_is_passed()
        {
            var expexted = UnixDateTime.Now();

            var ts = TimeStamp.From(DateTime.UtcNow);

            var r = new DateTime(ts.Ticks);
            r.Should().BeCloseTo(expexted);
        }

        [Test]
        public void Equals_Should_return_false_When_a_time_stamp_based_on_a_different_date_time_is_passed()
        {
            var now = DateTime.Now;
            var tsX = TimeStamp.From(now);
            var tsY = TimeStamp.From(now.AddTicks(-1));

            tsX.Equals(tsY).Should().BeFalse();
        }

        [Test]
        public void Equals_Should_return_true_When_a_time_stamp_based_on_same_date_time_is_passed()
        {
            var now = DateTime.Now;
            var tsX = TimeStamp.From(now);
            var tsY = TimeStamp.From(now);

            tsX.Equals(tsY).Should().BeTrue();
        }
    }
}