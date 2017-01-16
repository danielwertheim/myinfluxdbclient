using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [TestFixture]
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
        public async Task Should_return_all_series_When_no_from_or_where_is_specified()
        {
            var series = await Client.GetSeriesAsync(_databaseName);

            var orderCreatedItems = series.Where(s => s.Key.StartsWith("orderCreated")).ToList();
            var paymentRecievedItems = series.Where(s => s.Key.StartsWith("paymentRecieved")).ToList();

            orderCreatedItems.Should().HaveSameCount(_seededOrderCreatedKeys);
            paymentRecievedItems.Should().HaveSameCount(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_measurement_When_from_measurement_is_specified()
        {
            var orderCreatedItems = await Client.GetSeriesAsync(_databaseName, new ShowSeries().FromMeasurement("orderCreated"));
            orderCreatedItems.Should().OnlyContain(s => s.Key.StartsWith("orderCreated"));
            orderCreatedItems.Should().HaveSameCount(_seededOrderCreatedKeys);
            orderCreatedItems.Select(i => i.Key).Should().Contain(_seededOrderCreatedKeys);

            var paymentRecievedItems = await Client.GetSeriesAsync(_databaseName, new ShowSeries().FromMeasurement("paymentRecieved"));
            paymentRecievedItems.Should().OnlyContain(s => s.Key.StartsWith("paymentRecieved"));
            paymentRecievedItems.Should().HaveSameCount(_seededPaymentRecievedKeys);
            paymentRecievedItems.Select(i => i.Key).Should().Contain(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_tag_When_where_tags_are_specified()
        {
            var items = await Client.GetSeriesAsync(_databaseName, new ShowSeries().WhereTags("oid='1' and mid='1'"));

            items.Should().HaveCount(3);
            items.Select(i => i.Key).Should().Contain(new[] { "orderCreated,mid=1,oid=1", "paymentRecieved,mid=1,oid=1,pid=1", "paymentRecieved,mid=1,oid=1,pid=2" });
        }

        [Test]
        public async Task Should_return_by_measurement_and_tag_When_from_measurement_and_where_tag_are_specified()
        {
            var orderCreatedItems = await Client.GetSeriesAsync(_databaseName, new ShowSeries().FromMeasurement("orderCreated").WhereTags("mid='2'"));
            var paymentRecievedItems = await Client.GetSeriesAsync(_databaseName, new ShowSeries().FromMeasurement("paymentRecieved").WhereTags("oid='1' and mid='1'"));

            orderCreatedItems.Should().HaveCount(1);
            orderCreatedItems.Should().Contain(i => i.Key == "orderCreated,mid=2,oid=3");

            paymentRecievedItems.Should().HaveSameCount(_seededPaymentRecievedKeys);
            paymentRecievedItems.Select(i => i.Key).Should().Contain(_seededPaymentRecievedKeys);
        }
    }
}