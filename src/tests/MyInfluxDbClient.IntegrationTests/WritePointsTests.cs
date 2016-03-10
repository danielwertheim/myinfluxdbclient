using System.Threading.Tasks;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [TestFixture]
    public class WritePointsTests : IntegrationTests
    {
        [Test]
        public async Task Write_Should_write_points_When_passing_points()
        {
            var points = new InfluxPoints().Add(new InfluxPoint("WriteTest")
                .AddTag("Tag1", "alpha")
                .AddTag("Tag2", "beta")
                .AddField("myint", 1)
                .AddField("mylng", (long)2)
                .AddField("myflt", (float)3.15)
                .AddField("mydbl", 4.15)
                .AddField("mydec", 5.15M)
                .AddField("mybol", true)
                .AddField("mystr", "test")
                .AddTimeStamp(TimeStampResolutions.Milliseconds));

            await Client.WriteAsync(IntegrationTestsRuntime.DatabaseName, points);
        }
    }
}