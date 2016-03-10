using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [TestFixture]
    public class RetentionPolicyTests : IntegrationTests
    {
        private string _databaseName;

        protected override void OnBeforeEachTest()
        {
            _databaseName = CreateUniqueTestDatabase();
        }

        protected override void OnAfterEachTest()
        {
            DropTestDatabase(_databaseName);
        }

        [Test]
        public async Task Can_manage_retention_policies()
        {
            var createPolicy = new CreateRetentionPolicy("TestPolicy", RetentionPolicyDuration.Days(1), 1);

            await Client.CreateRetentionPolicyAsync(_databaseName, createPolicy);
            var policies = await Client.GetRetentionPoliciesAsync(_databaseName);

            var refetched = policies.SingleOrDefault(p => p.Name == createPolicy.Name);
            refetched.Should().NotBeNull();
            refetched.ReplicaN.Should().Be(createPolicy.Replication);
            refetched.IsDefault.Should().BeFalse();
            refetched.Duration.Should().NotBeNullOrWhiteSpace();

            var alterPolicy = new AlterRetentionPolicy(createPolicy.Name)
            {
                Replication = 2,
                Duration = RetentionPolicyDuration.Hours(10)
            };
            await Client.AlterRetentionPolicyAsync(_databaseName, alterPolicy);

            policies = await Client.GetRetentionPoliciesAsync(_databaseName);
            refetched = policies.SingleOrDefault(p => p.Name == createPolicy.Name);
            refetched.Should().NotBeNull();
            refetched.ReplicaN.Should().Be(alterPolicy.Replication);
            refetched.IsDefault.Should().BeFalse();
            refetched.Duration.Should().NotBeNullOrWhiteSpace();

            await Client.DropRetentionPolicyAsync(_databaseName, createPolicy.Name);
            policies = await Client.GetRetentionPoliciesAsync(_databaseName);
            policies.Should().NotContain(p => p.Name == createPolicy.Name);
        }
    }
}