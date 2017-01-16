using System;
using System.Linq;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [SetUpFixture]
    public class IntegrationTestsRuntime
    {
        public const string Host = "http://ubuntu01:8086";
        public const string DatabaseName = "int_tests_main";

        public static string GenerateUniqueDatabaseName()
        {
            return $"{DatabaseName}_{Guid.NewGuid().ToString("n")}";
        }

        public IntegrationTestsRuntime()
        {
            using (var client = new InfluxDbClient(Host))
            {
                var dbNames = client.GetDatabasesAsync().Result;
                foreach (var databaseName in dbNames.Where(dbName => dbName.StartsWith(DatabaseName)))
                    client.DropDatabaseAsync(databaseName).Wait();

                client.CreateDatabaseAsync(DatabaseName).Wait();
            }
        }
    }
}