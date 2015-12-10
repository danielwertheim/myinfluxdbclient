using System.Collections.Generic;

namespace MyInfluxDbClient.Responses
{
    public class SerieItem
    {
        public string Key { get; set; }
        public Dictionary<string, string> Tags { get; set; }
    }
}