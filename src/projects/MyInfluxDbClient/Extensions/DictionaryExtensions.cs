using System.Collections.Generic;
using System.Text;

namespace MyInfluxDbClient.Extensions
{
    internal static class DictionaryExtensions
    {
        public static void AppendTo(this Dictionary<string, string> kvs, StringBuilder sb)
        {
            foreach (var kv in kvs)
            {
                sb.Append(kv.Key);
                sb.Append("=");
                sb.Append(kv.Value);
                sb.Append(",");
            }

            sb.Remove(sb.Length - 1, 1);
        }
    }
}