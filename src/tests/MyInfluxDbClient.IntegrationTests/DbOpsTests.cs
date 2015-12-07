using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public class DbOpsTests : IntegrationTests
    {
        [Test]
        public async Task Can_manage_databases()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();

            await Client.CreateDatabaseAsync(databaseName);

            (await Client.DatabaseExistsAsync(databaseName)).Should().BeTrue();

            (await Client.GetDatabaseNamesAsync()).Should().Contain(databaseName);

            await Client.DropDatabaseAsync(databaseName);

            (await Client.DatabaseExistsAsync(databaseName)).Should().BeFalse();

            (await Client.GetDatabaseNamesAsync()).Should().NotContain(databaseName);
        }

        [Test]
        public async Task CreateDatabaseAsync_Should_throw_When_database_already_exists()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();

            await Client.CreateDatabaseAsync(databaseName);

            Client.Invoking(sut => Client.CreateDatabaseAsync(databaseName).Wait())
                .ShouldThrow<InfluxDbClientException>().Where(ex => ex.Reason == "database already exists");
        }

        [Test]
        public void DropDatabaseAsync_Should_throw_When_database_does_not_exist()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();

            Client.Invoking(sut => Client.DropDatabaseAsync(databaseName).Wait())
                .ShouldThrow<InfluxDbClientException>().Where(ex => ex.Reason.StartsWith("database not found"));
        }
    }
}