namespace MyInfluxDbClient
{
    public enum Consistency
    {
        One,
        Quorum,
        All,
        Any
    }

    internal static class ConsistencyExtensions
    {
        internal static string ToUrlValue(this Consistency value)
        {
            return value.ToString().ToLower();
        }
    }
}