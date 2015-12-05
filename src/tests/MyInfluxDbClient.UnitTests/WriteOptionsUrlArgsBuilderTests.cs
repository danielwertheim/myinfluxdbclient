using FluentAssertions;
using MyInfluxDbClient.Net;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class WriteOptionsUrlArgsBuilderTests : UnitTestsOf<WriteOptionsUrlArgsBuilder>
    {
        protected override void OnBeforeEachTest()
        {
            SUT = new WriteOptionsUrlArgsBuilder();
        }

        [Test]
        public void Build_Should_include_rp_value_When_specifying_retention_policy()
        {
            var options = new WriteOptions()
                .SetRetentionPolicy("test");

            SUT.Build(options).Should().Be("&rp=test");
        }

        [Test]
        public void Build_Should_encode_rp_value_When_specifying_retention_policy()
        {
            var options = new WriteOptions()
                .SetRetentionPolicy("test of this");

            SUT.Build(options).Should().Be("&rp=test%20of%20this");
        }

        [Test]
        public void Build_Should_include_consistency_value_When_specifying_consistency()
        {
            var options = new WriteOptions()
                .SetConsistency(Consistency.Quorum);

            SUT.Build(options).Should().Be("&consistency=quorum");
        }

        [Test]
        public void Build_Should_include_precision_value_When_specifying_time_stamp_precision()
        {
            var options = new WriteOptions()
                .SetTimeStampPrecision(TimeStampPrecision.Minutes);

            SUT.Build(options).Should().Be("&precision=m");
        }

        [Test]
        public void Build_Should_combine_url_args_from_all_passed_args_When_passing_all_args()
        {
            var options = new WriteOptions()
                .SetRetentionPolicy("test")
                .SetConsistency(Consistency.Quorum)
                .SetTimeStampPrecision(TimeStampPrecision.Minutes);

            SUT.Build(options).Should().Be("&rp=test&consistency=quorum&precision=m");
        }
    }
}