using System;
using FluentAssertions;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class DropSeriesTests : UnitTestsOf<DropSeries>
    {
        [Test]
        public void IsValid_Should_return_false_When_neither_From_nor_Where_has_been_defined()
        {
            SUT = new DropSeries();

            SUT.IsValid().Should().BeFalse();
        }

        [Test]
        public void IsValid_Should_return_true_When_either_From_or_Where_has_been_defined()
        {
            SUT = new DropSeries().FromMeasurement("test");
            SUT.IsValid().Should().BeTrue();

            SUT = new DropSeries().WhereTags("test='asdf'");
            SUT.IsValid().Should().BeTrue();
        }

        [Test]
        public void Generate_Should_return_drop_series_When_constructed_empty()
        {
            SUT = new DropSeries();

            SUT.Invoking(sut => sut.Generate()).ShouldThrow<InvalidOperationException>();
        }

        [Test]
        public void Generate_Should_return_drop_series_with_measurement_When_from_is_specified()
        {
            SUT = new DropSeries().FromMeasurement("orderCreated");

            SUT.Generate().Should().Be("drop series from \"orderCreated\"");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_where_When_where_is_specified()
        {
            SUT = new DropSeries().WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("drop series where merchant='foo'");
        }

        [Test]
        public void Generate_Should_return_drop_series_with_from_and_where_When_from_and_where_are_specified()
        {
            SUT = new DropSeries().FromMeasurement("orderCreated").WhereTags("merchant='foo'");

            SUT.Generate().Should().Be("drop series from \"orderCreated\" where merchant='foo'");
        }
    }
}