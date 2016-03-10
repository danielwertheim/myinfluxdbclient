using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    [TestFixture]
    public class NanosecondsResolutionTests : UnitTestsOf<NanosecondsResolution>
    {
        public NanosecondsResolutionTests()
        {
            SUT = new NanosecondsResolution();
        }

        [Test]
        public void NanosecondsFrom_Should_return_nanoseconds_by_multiplying_100_to_ticks()
        {
            var ts = TimeStamp.Now();

            var ns = SUT.NanosecondsFrom(ts);

            ns.Should().Be(ts.Ticks * 100);
        }

        [Test]
        public void NanosecondsFrom_Should_return_value_with_two_ending_zeroes()
        {
            var dt = new DateTime(635843473895094535);
            var ts = TimeStamp.From(dt);

            var ns = SUT.NanosecondsFrom(ts);

            var s = string.Join(string.Empty, ns.ToString().Reverse());
            s.Should().StartWith("00");
            s.Skip(2).First().Should().NotBe('0');
        }
    }
}