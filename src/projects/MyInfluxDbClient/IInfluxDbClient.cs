using System.Threading.Tasks;

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

        Task DropSeriesAsync(string databaseName, DropSeries query);
        Task<Series> GetSeriesAsync(string databaseName, GetSeries query = null);
        Task<string> GetSeriesJsonAsync(string databaseName, GetSeries query = null);

        Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null);
    }
}