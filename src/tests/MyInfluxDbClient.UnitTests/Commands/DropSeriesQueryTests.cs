using System;
using FluentAssertions;
using MyInfluxDbClient.Commands;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests.Commands
{
    public class DropSeriesQueryTests : UnitTestsOf<DropSeriesQuery>
    {
        [Test]
        public void IsValid_Should_return_false_When_neither_From_nor_Where_has_been_defined()
        {
            SUT = new DropSeriesQuery();

            SUT.IsValid().Should().BeFalse();
        }

        [Test]
        public void IsValid_Should_return_true_When_either_From_or_Where_has_been_defined()
        {
            SUT = new DropSeriesQuery().FromMeasurement("test");
            SUT.IsValid().Should().BeTrue();

            SUT = new DropSeriesQuery().WhereTags("test='asdf'");
            SUT.IsValid().Should().BeTrue();
        }

        [Test]
        public void Generate_Should_return_drop_series_When_constructed_empty()
        {
            SUT = new DropSeriesQuery();

            SUT.Invoking(sut => sut.Generate()).ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Generate_Should_return_drop_series_with_measurement_When_from_is_specified()
        {
            SUT = new DropSeriesQuery().FromMeasurement("orderCreated");

            SUT.Generate().Should().Be("drop series from \"orderCreated\"");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_where_When_where_is_specified()
        {
            SUT = new DropSeriesQuery().WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("drop series where merchant='foo'");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_from_and_where_When_from_and_where_are_specified()
        {
            SUT = new DropSeriesQuery().FromMeasurement("orderCreated").WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("drop series from \"orderCreated\" where merchant='foo'");
        }
    }
}