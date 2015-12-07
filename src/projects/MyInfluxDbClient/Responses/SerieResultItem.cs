using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MyInfluxDbClient.Responses
{
    internal class SerieResultItem
    {
        public string Name { get; set; }
        public List<string> Columns { get; set; } = new List<string>();
        public List<JToken> Values { get; set; } = new List<JToken>();
    }
}