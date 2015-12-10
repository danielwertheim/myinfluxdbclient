namespace MyInfluxDbClient
{
    public class DropSeriesQuery : SeriesQuery
    {
        public DropSeriesQuery() : base("drop series") {}
    }
}