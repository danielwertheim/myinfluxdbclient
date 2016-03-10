using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    [TestFixture]
    public class WriteOptionsTests : UnitTestsOf<WriteOptions>
    {
        protected override void OnBeforeEachTest()
        {
            SUT = new WriteOptions();
        }

        [Test]
        public void SetRetentionPolicy_Should_update_value_When_non_empty_string_is_passed()
        {
            SUT.SetRetentionPolicy("test");

            SUT.RetentionPolicy.Should().Be("test");
        }

        [Test]
        public void SetRetentionPolicy_Should_Throw_When_passing_null()
        {
            SUT.Invoking(sut => sut.SetRetentionPolicy(null)).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void SetRetentionPolicy_Should_Throw_When_passing_empty_string()
        {
            SUT.Invoking(sut => sut.SetRetentionPolicy(string.Empty)).ShouldThrow<ArgumentException>();
        }

        [Test]
        public void SetConsistency_Should_update_value()
        {
            SUT.SetConsistency(Consistency.One);

            SUT.Consistency.Should().Be(Consistency.One);
        }

        [Test]
        public void SetPrecision_Should_update_value()
        {
            SUT.SetTimeStampPrecision(TimeStampPrecision.Hours);

            SUT.TimeStampPrecision.Should().Be(TimeStampPrecision.Hours);
        }
    }
}