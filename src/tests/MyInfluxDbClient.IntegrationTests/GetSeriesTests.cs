using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using MyInfluxDbClient.Commands;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public class GetSeriesTests : IntegrationTests
    {
        private string _databaseName;
        private string[] _seededOrderCreatedKeys;
        private string[] _seededPaymentRecievedKeys;
        private InfluxPoints _seededPoints;

        protected override void OnBeforeAllTests()
        {
            _databaseName = CreateUniqueTestDatabase();

            _seededOrderCreatedKeys = new[] { "orderCreated,mid=1,oid=1", "orderCreated,mid=1,oid=2", "orderCreated,mid=2,oid=3" };
            _seededPaymentRecievedKeys = new[] { "paymentRecieved,mid=1,oid=1,pid=1", "paymentRecieved,mid=1,oid=1,pid=2" };
            _seededPoints = new InfluxPoints()
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 112.5))
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "2").AddTag("mid", "1").AddField("amt", 90M))
                .Add(new InfluxPoint("orderCreated").AddTag("oid", "3").AddTag("mid", "2").AddField("amt", 50M))
                .Add(new InfluxPoint("paymentRecieved").AddTag("pid", "1").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 60M))
                .Add(new InfluxPoint("paymentRecieved").AddTag("pid", "2").AddTag("oid", "1").AddTag("mid", "1").AddField("amt", 52.5M));

            Client.WriteAsync(_databaseName, _seededPoints).Wait();
        }

        protected override void OnAfterAllTests()
        {
            DropTestDatabase(_databaseName);
        }

        [Test]
        public async Task Should_map_key_and_tags()
        {
            var series = await Client.GetSeriesAsync(_databaseName);

            foreach (var serieItem in series["orderCreated"])
            {
                serieItem.Tags.Should().ContainKey("mid");
                serieItem.Tags.Should().ContainKey("oid");
            }

            foreach (var serieItem in series["paymentRecieved"])
            {
                serieItem.Tags.Should().ContainKey("mid");
                serieItem.Tags.Should().ContainKey("oid");
                serieItem.Tags.Should().ContainKey("pid");
            }
        }

        [Test]
        public async Task Should_return_all_series_When_no_from_or_where_is_specified()
        {
            var series = await Client.GetSeriesAsync(_databaseName);

            series.Should().ContainKey("orderCreated");
            series.Should().ContainKey("paymentRecieved");
            series["orderCreated"].Should().HaveSameCount(_seededOrderCreatedKeys);
            series["paymentRecieved"].Should().HaveSameCount(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_measurement_When_from_measurement_is_specified()
        {
            var orderCreatedItems = await Client.GetSeriesAsync(_databaseName, new GetSeriesQuery().FromMeasurement("orderCreated"));
            orderCreatedItems.Should().HaveCount(1);
            orderCreatedItems.Should().ContainKey("orderCreated");
            orderCreatedItems["orderCreated"].Should().HaveSameCount(_seededOrderCreatedKeys);
            orderCreatedItems["orderCreated"].Select(i => i.Key).Should().Contain(_seededOrderCreatedKeys);

            var paymentRecievedItems = await Client.GetSeriesAsync(_databaseName, new GetSeriesQuery().FromMeasurement("paymentRecieved"));
            paymentRecievedItems.Should().HaveCount(1);
            paymentRecievedItems.Should().ContainKey("paymentRecieved");
            paymentRecievedItems["paymentRecieved"].Should().HaveSameCount(_seededPaymentRecievedKeys);
            paymentRecievedItems["paymentRecieved"].Select(i => i.Key).Should().Contain(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_tag_When_where_tags_are_specified()
        {
            var items = await Client.GetSeriesAsync(_databaseName, new GetSeriesQuery().WhereTags("oid='1' and mid='1'"));

            var values = items.SelectMany(i => i.Value).ToArray();
            values.Should().HaveCount(3);
            values.Select(i => i.Key).Should().Contain(new[] { "orderCreated,mid=1,oid=1", "paymentRecieved,mid=1,oid=1,pid=1", "paymentRecieved,mid=1,oid=1,pid=2" });
        }

        [Test]
        public async Task Should_return_by_measurement_and_tag_When_from_measurement_and_where_tag_are_specified()
        {
            var orderCreatedItems = await Client.GetSeriesAsync(_databaseName, new GetSeriesQuery().FromMeasurement("orderCreated").WhereTags("mid='2'"));
            var paymentRecievedItems = await Client.GetSeriesAsync(_databaseName, new GetSeriesQuery().FromMeasurement("paymentRecieved").WhereTags("oid='1' and mid='1'"));

            orderCreatedItems["orderCreated"].Should().HaveCount(1);
            orderCreatedItems["orderCreated"].Should().Contain(i => i.Key == "orderCreated,mid=2,oid=3");

            paymentRecievedItems["paymentRecieved"].Should().HaveSameCount(_seededPaymentRecievedKeys);
            paymentRecievedItems["paymentRecieved"].Select(i => i.Key).Should().Contain(_seededPaymentRecievedKeys);
        }
    }
}