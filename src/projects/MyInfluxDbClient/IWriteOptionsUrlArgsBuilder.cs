namespace MyInfluxDbClient
{
    public interface IWriteOptionsUrlArgsBuilder {
        string Build(WriteOptions options);
    }
}