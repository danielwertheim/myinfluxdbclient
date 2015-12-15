using System;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public class DropSeriesTests : IntegrationTests
    {
        private string _databaseName;

        protected override void OnBeforeAllTests()
        {
            _databaseName = CreateUniqueTestDatabase();
        }

        [Test]
        public async Task Should_drop_series_When_series_matching_measurement_exists()
        {
            var measurementName = Guid.NewGuid().ToString("n");
            var points = new InfluxPoints()
                .Add(new InfluxPoint(measurementName).AddTag("tag1", "one").AddField("amt", 100M));
            await Client.WriteAsync(_databaseName, points);

            await Client.DropSeriesAsync(_databaseName, new DropSeries().FromMeasurement(measurementName));

            var series = await Client.GetSeriesAsync(_databaseName);
            series.Should().NotContainKey(measurementName);
        }

        [Test]
        public async Task Should_drop_series_When_series_matching_tags_exists()
        {
            var measurementName = Guid.NewGuid().ToString("n");
            var points = new InfluxPoints()
                .Add(new InfluxPoint(measurementName).AddTag("tag1", "one").AddField("amt", 100M));
            await Client.WriteAsync(_databaseName, points);

            await Client.DropSeriesAsync(_databaseName, new DropSeries().WhereTags("tag1 = 'one'"));

            var series = await Client.GetSeriesAsync(_databaseName);
            series.Should().NotContainKey(measurementName);
        }

        [Test]
        public async Task Should_not_throw_When_series_does_not_exist()
        {
            var measurementName = Guid.NewGuid().ToString("n");
            var points = new InfluxPoints()
                .Add(new InfluxPoint(measurementName).AddTag("tag1", "one").AddField("amt", 100M));
            await Client.WriteAsync(_databaseName, points);

            await Client.DropSeriesAsync(_databaseName, new DropSeries().FromMeasurement(measurementName + "foo"));

            var series = await Client.GetSeriesAsync(_databaseName);
            series.Should().ContainKey(measurementName);
        }
    }
}