using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsureThat;
using Requester.Http;

namespace InfluxDbClient
{
    public class InfluxPoints
    {
        private readonly List<InfluxPoint> _measurements = new List<InfluxPoint>();

        public int Count => _measurements.Count;

        public bool IsEmpty => !_measurements.Any();

        public InfluxPoints Add(InfluxPoint point)
        {
            Ensure.That(point, nameof(point)).IsNotNull();

            _measurements.Add(point);

            return this;
        }

        public InfluxPoints Add(IEnumerable<InfluxPoint> measurements)
        {
            Ensure.That(measurements, nameof(measurements)).IsNotNull();

            _measurements.AddRange(measurements);

            return this;
        }

        public void Clear()
        {
            _measurements.Clear();
        }

        public BytesContent ToBytesContent()
        {
            var sb = new StringBuilder();
            var m = _measurements.Count - 1;
            for (var index = 0; index < _measurements.Count; index++)
            {
                sb.Append(_measurements[index].GenerateLine());
                if(index < m)
                    sb.Append(InfluxDbEnvironment.NewLine);
            }

            return new BytesContent(Encoding.UTF8.GetBytes(sb.ToString()), HttpContentTypes.Instance.Text);
        }
    }
}