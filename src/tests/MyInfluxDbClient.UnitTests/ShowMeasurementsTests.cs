using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    [TestFixture]
    public class ShowMeasurementsTests : UnitTestsOf<ShowMeasurements>
    {
        [Test]
        public void Generate_Should_return_show_measurements_When_constructed_empty()
        {
            SUT = new ShowMeasurements();

            SUT.Generate().Should().Be("show measurements");
        }

        [Test]
        public void Generate_Should_return_show_measurements_with_where_When_where_is_specified()
        {
            SUT = new ShowMeasurements().WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("show measurements where merchant='foo'");
        }
    }
}