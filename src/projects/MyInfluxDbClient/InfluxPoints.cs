using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EnsureThat;

namespace MyInfluxDbClient
{
    public class InfluxPoints : IEnumerable<InfluxPoint>
    {
        private readonly List<InfluxPoint> _points = new List<InfluxPoint>();

        public InfluxPoint this[int index] => _points[index];

        public int Count => _points.Count;

        public bool IsEmpty => !_points.Any();

        public bool ShouldValidatePoints { get; private set; }

        public InfluxPoints()
        {
            ShouldValidatePoints = true;
        }

        public InfluxPoints Add(InfluxPoint point)
        {
            _points.Add(EnsurePointIsOkForAdding(point));

            return this;
        }

        public InfluxPoints Add(IEnumerable<InfluxPoint> points)
        {
            Ensure.That(points, nameof(points)).IsNotNull();

            _points.AddRange(EnsurePointsAreOkForAdding(points));

            return this;
        }

        protected IEnumerable<InfluxPoint> EnsurePointsAreOkForAdding(IEnumerable<InfluxPoint> points)
        {
            return points.Select(EnsurePointIsOkForAdding);
        }

        protected InfluxPoint EnsurePointIsOkForAdding(InfluxPoint point)
        {
            Ensure.That(point, nameof(point)).IsNotNull();

            if (!ShouldValidatePoints)
                return point;

            Ensure.That(point.IsComplete(), nameof(point))
                .WithExtraMessageOf(() => "Point is not valid. Ensure minimum information exists (measurement and one field value).")
                .IsTrue();

            return point;
        }

        public void Clear()
        {
            _points.Clear();
        }

        public void DisablePointValidation()
        {
            ShouldValidatePoints = false;
        }

        public IEnumerator<InfluxPoint> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _points.GetEnumerator();
        }
    }
}