using System;
using FluentAssertions;
using MyInfluxDbClient.Commands;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests.Commands
{
    public class RetentionPolicyTests : UnitTestsOf<CreateRetentionPolicy>
    {
        [Test]
        public void Constructor_Should_throw_When_name_is_missing()
        {
            Action a = () => new CreateRetentionPolicy(null, RetentionPolicyDuration.Days(1), 1);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("name");
        }

        [Test]
        public void Constructor_Should_throw_When_duration_is_missing()
        {
            Action a = () => new CreateRetentionPolicy("fake", null, 1);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("duration");
        }

        [Test]
        public void Constructor_Should_throw_When_replicatoin_is_zero()
        {
            Action a = () => new CreateRetentionPolicy("fake", RetentionPolicyDuration.Days(1), 0);

            a.ShouldThrow<ArgumentException>()
                .And.ParamName.Should().Be("replication");
        }
    }
}