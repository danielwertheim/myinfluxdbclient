using System.Threading.Tasks;

namespace MyInfluxDbClient
{
    public interface IInfluxDbClient
    {
        WriteOptions DefaultWriteOptions { get; }

        Task CreateDbAsync(string dbName);
        Task WriteAsync(string dbName, InfluxPoints points, WriteOptions options = null);
    }
}