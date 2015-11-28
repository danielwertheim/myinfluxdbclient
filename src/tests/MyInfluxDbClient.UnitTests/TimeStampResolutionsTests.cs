using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class TimeStampResolutionsTests : UnitTests
    {
        [Test]
        public void Default_Should_be_of_NanosecondsResolutionType()
        {
            TimeStampResolutions.Default.Should().BeOfType<NanosecondsResolution>();
        }

        [Test]
        public void Nanoseconds_Should_be_of_NanosecondsResolutionType()
        {
            TimeStampResolutions.Nanoseconds.Should().BeOfType<NanosecondsResolution>();
        }

        [Test]
        public void Microseconds_Should_be_of_MicrosecondsResolutionType()
        {
            TimeStampResolutions.Microseconds.Should().BeOfType<MicrosecondsResolution>();
        }

        [Test]
        public void Milliseconds_Should_be_of_MillisecondsResolutionType()
        {
            TimeStampResolutions.Milliseconds.Should().BeOfType<MillisecondsResolution>();
        }

        [Test]
        public void Seconds_Should_be_of_SecondsResolutionType()
        {
            TimeStampResolutions.Seconds.Should().BeOfType<SecondsResolution>();
        }

        [Test]
        public void Minutes_Should_be_of_MinutesResolutionType()
        {
            TimeStampResolutions.Minutes.Should().BeOfType<MinuteResolution>();
        }

        [Test]
        public void Hours_Should_be_of_HoursResolutionType()
        {
            TimeStampResolutions.Hours.Should().BeOfType<HourResolution>();
        }
    }
}