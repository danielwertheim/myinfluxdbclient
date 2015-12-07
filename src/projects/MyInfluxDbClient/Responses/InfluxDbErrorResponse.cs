using System.Collections.Generic;

namespace MyInfluxDbClient.Responses
{
    internal class InfluxDbErrorResponse
    {
        public List<ErrorResult> Results { get; set; } = new List<ErrorResult>();
    }
}