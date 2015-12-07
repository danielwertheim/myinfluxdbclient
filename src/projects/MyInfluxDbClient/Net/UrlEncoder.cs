using System;

namespace MyInfluxDbClient.Net
{
    internal static class UrlEncoder
    {
        internal static string Encode(string value)
        {
            return Uri.EscapeDataString(value);
        }
    }
}