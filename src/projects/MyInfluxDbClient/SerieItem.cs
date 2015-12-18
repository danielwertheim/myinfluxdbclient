namespace MyInfluxDbClient
{
    public class SerieItem
    {
        public string Key { get; set; }
        public Tags Tags { get; set; } = new Tags();
    }
}