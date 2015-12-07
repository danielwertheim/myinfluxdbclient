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

        Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null);
    }
}