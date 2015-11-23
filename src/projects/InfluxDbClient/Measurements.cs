using System.Collections.Generic;
using System.Linq;
using System.Text;
using EnsureThat;
using Requester.Http;

namespace InfluxDbClient
{
    public class Measurements
    {
        private readonly List<Measurement> _measurements = new List<Measurement>();

        public int Count => _measurements.Count;

        public bool IsEmpty => !_measurements.Any();

        public Measurements Add(Measurement measurement)
        {
            Ensure.That(measurement, nameof(measurement)).IsNotNull();

            _measurements.Add(measurement);

            return this;
        }

        public Measurements Add(IEnumerable<Measurement> measurements)
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