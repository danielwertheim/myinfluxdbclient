namespace MyInfluxDbClient
{
    public class ShowSeries : SeriesQuery<ShowSeries>
    {
        public ShowSeries() : base("show series") { }
    }
}