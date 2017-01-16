using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [TestFixture]
    public class DbOpsTests : IntegrationTests
    {
        [Test]
        public async Task Can_manage_databases()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();

            await Client.CreateDatabaseAsync(databaseName);

            (await Client.DatabaseExistsAsync(databaseName)).Should().BeTrue();

            (await Client.GetDatabasesAsync()).Should().Contain(databaseName);

            await Client.DropDatabaseAsync(databaseName);

            (await Client.DatabaseExistsAsync(databaseName)).Should().BeFalse();

            (await Client.GetDatabasesAsync()).Should().NotContain(databaseName);
        }

        [Test]
        public async Task CreateDatabaseAsync_Should_not_throw_When_database_already_exists()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();
            await Client.CreateDatabaseAsync(databaseName);

            Client.Invoking(sut => sut.CreateDatabaseAsync(databaseName).Wait()).ShouldNotThrow();
        }

        [Test]
        public void DropDatabaseAsync_Should_not_throw_When_database_does_not_exist()
        {
            var databaseName = IntegrationTestsRuntime.GenerateUniqueDatabaseName();

            Client.Invoking(sut => sut.DropDatabaseAsync(databaseName).Wait()).ShouldNotThrow();
        }
    }
}