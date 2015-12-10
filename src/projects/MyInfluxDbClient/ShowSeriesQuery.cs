namespace MyInfluxDbClient
{
    public class ShowSeriesQuery : SeriesQuery
    {
        public ShowSeriesQuery() : base("show series") { }
    }
}