namespace MyInfluxDbClient
{
    public class GetSeries : SeriesQuery<GetSeries>
    {
        public GetSeries() : base("show series") { }
    }
}