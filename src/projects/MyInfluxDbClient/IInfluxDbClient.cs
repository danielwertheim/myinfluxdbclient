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

        Task DropSeriesAsync(string databaseName, DropSeries command);
        Task<Series> GetSeriesAsync(string databaseName, ShowSeries command = null);
        Task<string> GetSeriesJsonAsync(string databaseName, ShowSeries command = null);

        Task<FieldKeys> GetFieldKeysAsync(string databaseName, string measurement = null);
        Task<string> GetFieldKeysJsonAsync(string databaseName, string measurement = null);

        Task<TagKeys> GetTagKeysAsync(string databaseName, string measurement = null);
        Task<string> GetTagKeysJsonAsync(string databaseName, string measurement = null);

        Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null);
    }
}