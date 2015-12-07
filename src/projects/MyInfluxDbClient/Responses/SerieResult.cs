using System.Collections.Generic;

namespace MyInfluxDbClient.Responses
{
    internal class SerieResult
    {
        public List<SerieResultItem> Series { get; set; } = new List<SerieResultItem>();
    }
}