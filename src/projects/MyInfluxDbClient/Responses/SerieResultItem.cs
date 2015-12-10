using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace MyInfluxDbClient.Responses
{
    internal class SerieResultItem
    {
        public string Name { get; set; }
        public List<string> Columns { get; set; } = new List<string>();
        public List<JToken> Values { get; set; } = new List<JToken>();

        public Dictionary<string, int> GetSchemaOrdinals()
        {
            var schema = new Dictionary<string, int>();

            for (var ci = 0; ci < Columns.Count; ci++)
                schema.Add(Columns[ci], ci);

            return schema;
        }
    }
}