namespace MyInfluxDbClient
{
    public interface IQuery
    {
        string From { get; }
        string Where { get; }

        string Generate();
    }
}