using System.Collections.Generic;

namespace MyInfluxDbClient.Responses
{
    internal class InfluxDbResponse
    {
        public List<SerieResult> Results { get; set; } = new List<SerieResult>();
    }
}