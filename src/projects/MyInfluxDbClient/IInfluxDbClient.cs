using System.Threading.Tasks;
using MyInfluxDbClient.Commands;
using MyInfluxDbClient.Responses;

namespace MyInfluxDbClient
{
    public interface IInfluxDbClient
    {
        WriteOptions DefaultWriteOptions { get; }

        Task CreateDatabaseAsync(string databaseName);
        Task DropDatabaseAsync(string databaseName);
        Task<bool> DatabaseExistsAsync(string databaseName);
        Task<string[]> GetDatabaseNamesAsync();

        Task CreateRetentionPolicyAsync(string databaseName, CreateRetentionPolicy policy);
        Task AlterRetentionPolicyAsync(string databaseName, AlterRetentionPolicy policy);
        Task DropRetentionPolicyAsync(string databaseName, string policyName);
        Task<RetentionPolicyItem[]> GetRetentionPoliciesAsync(string databaseName);
        Task<string> GetRetentionPoliciesJsonAsync(string databaseName);

        Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null);
    }
}