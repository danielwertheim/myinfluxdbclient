using System;
using System.Collections.Generic;
using System.Linq;

namespace MyInfluxDbClient.UnitTests.TestData
{
    internal class InfluxPointsTestData
    {
        internal static readonly InfluxPointsTestData Instance = new InfluxPointsTestData();

        internal InfluxPoint CreateDefault()
        {
            return new InfluxPoint("TestMeasurement");
        }

        internal InfluxPoint CreateIncomplete()
        {
            return CreateDefault();
        }

        internal InfluxPoint CreateSimplest()
        {
            return CreateDefault().AddField("Field1", 1M);
        }

        internal List<InfluxPoint> CreateMany(int numOfPoints, Func<InfluxPointsTestData, Func<InfluxPoint>> fn)
        {
            var pointFn = fn == null ? CreateDefault : fn(Instance);

            return Enumerable.Range(1, numOfPoints).Select(i =>
            {
                var point = pointFn();

                return pointFn == CreateIncomplete
                    ? point
                    : point.AddField("Index", i);
            }).ToList();
        }
    }
}