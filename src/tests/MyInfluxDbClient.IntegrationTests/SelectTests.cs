using System.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [TestFixture]
    public class SelectTests : IntegrationTests
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
        public async Task Should_extract_key_and_tags_by_measurement()
        {
            var matches = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("orderCreated"));

            foreach (var match in matches["orderCreated"])
            {
                match.Tags.Should().ContainKey("mid");
                match.Tags.Should().ContainKey("oid");
            }

            matches = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("paymentRecieved"));
            foreach (var match in matches["paymentRecieved"])
            {
                match.Tags.Should().ContainKey("mid");
                match.Tags.Should().ContainKey("oid");
                match.Tags.Should().ContainKey("pid");
            }
        }

        [Test]
        public async Task Should_return_all_series_When_no_from_or_where_is_specified()
        {
            var matches = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("orderCreated"));
            matches.Should().ContainKey("orderCreated");
            matches["orderCreated"].Should().HaveSameCount(_seededOrderCreatedKeys);

            matches = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("paymentRecieved"));
            matches.Should().ContainKey("paymentRecieved");
            matches["paymentRecieved"].Should().HaveSameCount(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_measurement_When_from_measurement_is_specified()
        {
            var orderCreatedItems = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("orderCreated"));
            orderCreatedItems.Should().HaveCount(1);
            orderCreatedItems.Should().ContainKey("orderCreated");
            orderCreatedItems["orderCreated"].Should().HaveSameCount(_seededOrderCreatedKeys);

            var paymentRecievedItems = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("paymentRecieved"));
            paymentRecievedItems.Should().HaveCount(1);
            paymentRecievedItems.Should().ContainKey("paymentRecieved");
            paymentRecievedItems["paymentRecieved"].Should().HaveSameCount(_seededPaymentRecievedKeys);
        }

        [Test]
        public async Task Should_return_by_measurement_and_tag_When_from_measurement_and_where_tag_are_specified()
        {
            var orderCreatedItems = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("orderCreated").WhereTags("mid='2'"));
            orderCreatedItems["orderCreated"].Should().HaveCount(1);

            var paymentRecievedItems = await Client.SelectAsync(_databaseName, Select.All().FromMeasurement("paymentRecieved").WhereTags("oid='1' and mid='1' and pid='2'"));
            paymentRecievedItems["paymentRecieved"].Should().HaveCount(1);
        }
    }
}