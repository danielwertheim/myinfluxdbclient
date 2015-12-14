namespace MyInfluxDbClient.Commands
{
    public class GetSeriesQuery : SeriesQuery<GetSeriesQuery>
    {
        public GetSeriesQuery() : base("show series") { }
    }
}