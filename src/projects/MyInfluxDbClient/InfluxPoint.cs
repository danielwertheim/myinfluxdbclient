using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsureThat;
using MyInfluxDbClient.Extensions;

namespace MyInfluxDbClient
{
    public class InfluxPoint
    {
        private readonly Dictionary<string, string> _tags = new Dictionary<string, string>();
        private readonly Dictionary<string, string> _fields = new Dictionary<string, string>();
        private TimeStamp? _timeStamp;
        private TimeStampResolution _timeStampResolution;

        public string Name { get; }

        public InfluxPoint(string name)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            Name = name;
        }

        public bool IsComplete()
        {
            return !string.IsNullOrWhiteSpace(Name) && _fields.Any();
        }

        public InfluxPoint AddTag(string name, IConvertible value)
        {
            return AddRawTag(name, value.ToString(InfluxDbEnvironment.FormatProvider));
        }

        public InfluxPoint AddTag(string name, string value)
        {
            return AddRawTag(name, string.IsNullOrEmpty(value)
                ? value
                : value.Replace("\"", string.Empty).Replace(" ", "\\ ").Replace(",", "\\,"));
        }

        public InfluxPoint AddRawTag(string name, string value)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            _tags.Add(name, value);

            return this;
        }

        public InfluxPoint AddField(string name, bool value)
        {
            return AddRawField(name, value ? InfluxDbEnvironment.Fields.TrueString : InfluxDbEnvironment.Fields.FalseString);
        }

        public InfluxPoint AddField(string name, int value)
        {
            return AddRawField(name, value.ToString(InfluxDbEnvironment.FormatProvider) + InfluxDbEnvironment.Fields.IntegerSuffix);
        }

        public InfluxPoint AddField(string name, long value)
        {
            return AddRawField(name, value.ToString(InfluxDbEnvironment.FormatProvider) + InfluxDbEnvironment.Fields.IntegerSuffix);
        }

        public InfluxPoint AddField(string name, float value)
        {
            return AddRawField(name, value.ToString(InfluxDbEnvironment.FormatProvider));
        }

        public InfluxPoint AddField(string name, double value)
        {
            return AddRawField(name, value.ToString(InfluxDbEnvironment.FormatProvider));
        }

        public InfluxPoint AddField(string name, decimal value)
        {
            return AddRawField(name, value.ToString(InfluxDbEnvironment.FormatProvider));
        }

        public InfluxPoint AddField(string name, string value)
        {
            return AddRawField(name, $"\"{value ?? string.Empty}\"");
        }

        public InfluxPoint AddRawField(string name, string value)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            _fields.Add(name, value);

            return this;
        }

        public InfluxPoint AddTimeStamp(TimeStampResolution resolution = null)
        {
            _timeStamp = TimeStamp.Now();
            _timeStampResolution = resolution ?? TimeStampResolution.Nanoseconds;

            return this;
        }

        public InfluxPoint AddTimeStamp(DateTime value, TimeStampResolution resolution = null)
        {
            _timeStamp = TimeStamp.From(value);
            _timeStampResolution = resolution ?? TimeStampResolution.Nanoseconds;

            return this;
        }

        public string GenerateLine()
        {
            var sb = new StringBuilder();
            sb.Append(Name);
            sb.Append(",");
            _tags.AppendTo(sb);
            sb.Append(" ");
            _fields.AppendTo(sb);

            if (_timeStamp.HasValue)
            {
                sb.Append(" ");
                sb.Append(_timeStampResolution.NanosecondsFrom(_timeStamp.Value));
            }

            return sb.ToString();
        }
    }
}