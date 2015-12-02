using System;
using System.Globalization;
using System.Text;

namespace MyInfluxDbClient.Protocols
{
    public static class LineProtocolFormat
    {
        public static class Fields
        {
            public const string TrueString = "t";
            public const string FalseString = "f";
            public const string IntegerSuffix = "i";
        }

        public static readonly string NewLine = new string(new[] { (char)10 });
        public static readonly IFormatProvider FormatProvider = CultureInfo.InvariantCulture;
        public static readonly Encoding Encoding = Encoding.UTF8;
    }
}