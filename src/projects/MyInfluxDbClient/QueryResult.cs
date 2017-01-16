using System.Collections.Generic;
using EnsureThat;

namespace MyInfluxDbClient
{
    public class QueryResult : Dictionary<string, QueryResultItem[]>
    {
        //public string Name { get; private set; }

        //public QueryResult(string name)
        //{
        //    EnsureArg.IsNotNullOrWhiteSpace(name, nameof(name));

        //    Name = name;
        //}
    }
}