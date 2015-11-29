using System.Collections.Generic;
using System.Text;
using EnsureThat;

namespace MyInfluxDbClient.Protocols
{
    public class StringBuilderLineProtocolWriter
    {
        private readonly StringBuilder _target;

        public StringBuilderLineProtocolWriter(StringBuilder target)
        {
            Ensure.That(target, nameof(target)).IsNotNull();

            _target = target;
        }

        public void Write(InfluxPoints points)
        {
            Ensure.That(points, nameof(points)).IsNotNull();

            var lastIndex = points.Count - 1;
            for (var index = 0; index < points.Count; index++)
            {
                Write(points[index]);
                if (index < lastIndex)
                    _target.Append(LineProtocolFormat.NewLine);
            }
        }

        private void Write(InfluxPoint point)
        {
            _target.Append(point.Measurement);
            _target.Append(",");
            Write(point.Tags);
            _target.Append(" ");
            Write(point.Fields);

            if (point.TimeStamp != null)
            {
                _target.Append(" ");
                _target.Append(point.TimeStampResolution.NanosecondsFrom(point.TimeStamp));
            }
        }

        private void Write(IEnumerable<KeyValuePair<string, string>> kvs)
        {
            foreach (var kv in kvs)
            {
                _target.Append(kv.Key);
                _target.Append("=");
                _target.Append(kv.Value);
                _target.Append(",");
            }

            _target.Remove(_target.Length - 1, 1);
        }
    }
}