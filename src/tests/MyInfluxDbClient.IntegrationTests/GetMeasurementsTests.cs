using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public class GetMeasurementsTests : IntegrationTests
    {
        private string _databaseName;
        private InfluxPoints _seededPoints;

        protected override void OnBeforeAllTests()
        {
            _databaseName = CreateUniqueTestDatabase();

            _seededPoints = new InfluxPoints()
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 112.5).AddField("fee", 10M))
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "2").AddTag("mid", "1").AddField("amt", 90M).AddField("fee", 10M))
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "3").AddTag("mid", "2").AddField("amt", 50M).AddField("fee", 10M))
                .Add(new InfluxPoint("paymentRecieved").AddTag("pid", "1").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 60M))
                .Add(new InfluxPoint("paymentRecieved").AddTag("pid", "2").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 52.5M));

            Client.WriteAsync(_databaseName, _seededPoints).Wait();
        }

        protected override void OnAfterAllTests()
        {
            DropTestDatabase(_databaseName);
        }

        [Test]
        public async Task Should_return_all_measurements_When_no_tags_are_specified()
        {
            var measurements = await Client.GetMeasurementsAsync(_databaseName);

            measurements.Should().Contain(new[] {"orderCreated", "paymentRecieved"});
        }

        [Test]
        public async Task Should_return_measurements_matching_tags_When_tags_are_specified()
        {
            var measurements = await Client.GetMeasurementsAsync(_databaseName, new ShowMeasurements().WhereTags("oid = '1'"));
            measurements.Should().Contain(new[] { "orderCreated", "paymentRecieved" });

            measurements = await Client.GetMeasurementsAsync(_databaseName, new ShowMeasurements().WhereTags("pid = '1'"));
            measurements.Should().Contain(new[] { "paymentRecieved" });
        }
    }
}