using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    [TestFixture]
    public class RetentionPolicyDurationTests : UnitTestsOf<RetentionPolicyDuration>
    {
        [Test]
        public void Should_return_value_with_m_suffix_string_When_calling_Minutes()
        {
            RetentionPolicyDuration.Minutes(1).Should().Be("1m");
            RetentionPolicyDuration.Minutes(10).Should().Be("10m");
            RetentionPolicyDuration.Minutes(120).Should().Be("120m");
        }

        [Test]
        public void Should_return_value_with_h_suffix_string_When_calling_Hours()
        {
            RetentionPolicyDuration.Hours(1).Should().Be("1h");
            RetentionPolicyDuration.Hours(10).Should().Be("10h");
            RetentionPolicyDuration.Hours(120).Should().Be("120h");
        }

        [Test]
        public void Should_return_value_with_d_suffix_string_When_calling_Days()
        {
            RetentionPolicyDuration.Days(1).Should().Be("1d");
            RetentionPolicyDuration.Days(10).Should().Be("10d");
            RetentionPolicyDuration.Days(120).Should().Be("120d");
        }

        [Test]
        public void Should_return_value_with_w_suffix_string_When_calling_Weeks()
        {
            RetentionPolicyDuration.Weeks(1).Should().Be("1w");
            RetentionPolicyDuration.Weeks(10).Should().Be("10w");
            RetentionPolicyDuration.Weeks(120).Should().Be("120w");
        }

        [Test]
        public void Should_return_value_INF_When_calling_Infinite()
        {
            RetentionPolicyDuration.Infinite().Should().Be("INF");
        }

        [Test]
        public void Should_throw_When_passing_zero_Minutes()
        {
            Action a = () => RetentionPolicyDuration.Minutes(0);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("value");
        }

        [Test]
        public void Should_throw_When_passing_zero_Hours()
        {
            Action a = () => RetentionPolicyDuration.Hours(0);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("value");
        }

        [Test]
        public void Should_throw_When_passing_zero_Days()
        {
            Action a = () => RetentionPolicyDuration.Days(0);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("value");
        }

        [Test]
        public void Should_throw_When_passing_zero_Weeks()
        {
            Action a = () => RetentionPolicyDuration.Weeks(0);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("value");
        }
    }
}