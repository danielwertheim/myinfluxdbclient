using System;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;
using MyInfluxDbClient.Protocols;

namespace MyInfluxDbClient
{
    public class InfluxPoint
    {
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _fields = new Dictionary<string, string>();

        public string Measurement { get; }
        public TimeStamp TimeStamp { get; private set; }
        public TimeStampResolution TimeStampResolution { get; private set; }
        public IEnumerable<KeyValuePair<string, string>> Tags => _tags;
        public IEnumerable<KeyValuePair<string, string>> Fields => _fields;

        public InfluxPoint(string measurement)
        {
            Ensure.That(measurement, nameof(measurement)).IsNotNullOrWhiteSpace();

            Measurement = EscapeStringValue(measurement);
        }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(Measurement) && _fields.Any();
        }

        public InfluxPoint AddTag(string name, IConvertible value)
        {
            return AddRawTag(name, value.ToString(LineProtocolFormat.FormatProvider));
        }

        public InfluxPoint AddTag(string name, string value)
        {
            return AddRawTag(name, string.IsNullOrEmpty(value)
                ? value
                : EscapeStringValue(value));
        }

        private InfluxPoint AddRawTag(string name, string value)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            _tags.Add(EscapeStringValue(name), value);

            return this;
        }

        public InfluxPoint AddField(string name, bool value)
        {
            return AddRawField(name, value ? LineProtocolFormat.Fields.TrueString : LineProtocolFormat.Fields.FalseString);
        }

        public InfluxPoint AddField(string name, int value)
        {
            return AddRawField(name, value.ToString(LineProtocolFormat.FormatProvider) + LineProtocolFormat.Fields.IntegerSuffix);
        }

        public InfluxPoint AddField(string name, long value)
        {
            return AddRawField(name, value.ToString(LineProtocolFormat.FormatProvider) + LineProtocolFormat.Fields.IntegerSuffix);
        }

        public InfluxPoint AddField(string name, float value)
        {
            return AddRawField(name, value.ToString(LineProtocolFormat.FormatProvider));
        }

        public InfluxPoint AddField(string name, double value)
        {
            return AddRawField(name, value.ToString(LineProtocolFormat.FormatProvider));
        }

        public InfluxPoint AddField(string name, decimal value)
        {
            return AddRawField(name, value.ToString(LineProtocolFormat.FormatProvider));
        }

        public InfluxPoint AddField(string name, string value)
        {
            if(!string.IsNullOrEmpty(value))
                value = EscapeStringValue(value);

            return AddRawField(name, $"\"{value ?? string.Empty}\"");
        }

        private InfluxPoint AddRawField(string name, string value)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            _fields.Add(EscapeStringValue(name), value);

            return this;
        }

        public InfluxPoint AddTimeStamp(TimeStampResolution resolution = null)
        {
            TimeStamp = TimeStamp.Now();
            TimeStampResolution = resolution ?? TimeStampResolutions.Default;

            return this;
        }

        public InfluxPoint AddTimeStamp(DateTime value, TimeStampResolution resolution = null)
        {
            TimeStamp = TimeStamp.From(value);
            TimeStampResolution = resolution ?? TimeStampResolutions.Default;

            return this;
        }

        private static string EscapeStringValue(string value)
        {
            return value.Replace("\"", "\\\"").Replace(" ", "\\ ").Replace(",", "\\,");
        }
    }
}