using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    [TestFixture]
    public class TimeStampTests : UnitTests
    {
        [Test]
        public void Now_Should_return_TimeStamp_with_UTC_ticks_from_epoch()
        {
            var expexted = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            var ts = TimeStamp.Now();

            ts.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
        }

        [Test]
        public void From_Should_give_TimeStamp_with_UTC_ticks_from_epoch_When_a_non_UTC_date_time_is_passed()
        {
            var expexted = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            var ts = TimeStamp.From(DateTime.Now);

            ts.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
        }

        [Test]
        public void From_Should_give_TimeStamp_with_UTC_ticks_from_epoch_When_an_UTC_date_time_is_passed()
        {
            var expexted = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            var ts = TimeStamp.From(DateTime.UtcNow);

            ts.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
        }

        [Test]
        public void ToUtcDateTime_Should_give_the_DateTime_used_to_create_it_as_UTC_When_non_UTC_was_passed()
        {
            var expected = DateTime.Now;
            var timeStamp = TimeStamp.From(expected);

            timeStamp
                .ToUtcDateTime()
                .ToLocalTime()
                .Should().Be(expected);
        }

        [Test]
        public void ToUtcDateTime_Should_give_the_DateTime_used_to_create_it_When_UTC_was_passed()
        {
            var expected = DateTime.UtcNow;
            var timeStamp = TimeStamp.From(expected);

            timeStamp
                .ToUtcDateTime()
                .Should()
                .Be(expected);
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