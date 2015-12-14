using FluentAssertions;
using MyInfluxDbClient.Commands;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests.Commands
{
    public class GetSeriesQueryTests : UnitTestsOf<GetSeriesQuery>
    {
        [Test]
        public void Generate_Should_return_drop_series_When_constructed_empty()
        {
            SUT = new GetSeriesQuery();

            SUT.Generate().Should().Be("show series");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_measurement_When_from_is_specified()
        {
            SUT = new GetSeriesQuery().FromMeasurement("orderCreated");

            SUT.Generate().Should().Be("show series from \"orderCreated\"");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_where_When_where_is_specified()
        {
            SUT = new GetSeriesQuery().WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("show series where merchant='foo'");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_from_and_where_When_from_and_where_are_specified()
        {
            SUT = new GetSeriesQuery().FromMeasurement("orderCreated").WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("show series from \"orderCreated\" where merchant='foo'");
        }
    }
}