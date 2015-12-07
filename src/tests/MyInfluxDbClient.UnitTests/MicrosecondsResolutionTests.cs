using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class MicrosecondsResolutionTests : UnitTestsOf<MicrosecondsResolution>
    {
        public MicrosecondsResolutionTests()
        {
            SUT = new MicrosecondsResolution();
        }

        [Test]
        public void NanosecondsFrom_Should_return_nanoseconds_by_multiplying_100_to_reminders_of_ticks_subtracted_by_modulus_10()
        {
            var ts = TimeStamp.Now();

            var ns = SUT.NanosecondsFrom(ts);

            ns.Should().Be((ts.Ticks - ts.Ticks % 10) * 100);
        }

        [Test]
        public void NanosecondsFrom_Should_return_value_with_three_ending_zeroes()
        {
            var dt = new DateTime(635843473895094535);
            var ts = TimeStamp.From(dt);

            var ns = SUT.NanosecondsFrom(ts);

            var s = string.Join(string.Empty, ns.ToString().Reverse());
            s.Should().StartWith("000");
            s.Skip(3).First().Should().NotBe('0');
        }
    }
}