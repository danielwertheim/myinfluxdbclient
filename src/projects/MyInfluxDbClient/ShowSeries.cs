namespace MyInfluxDbClient
{
    public class ShowSeries : Query<ShowSeries>
    {
        public ShowSeries() : base("show series") { }
    }
}