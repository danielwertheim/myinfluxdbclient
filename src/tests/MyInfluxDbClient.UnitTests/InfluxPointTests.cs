using System;
using FluentAssertions;
using MyInfluxDbClient.UnitTests.Shoulds;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class InfluxPointTests : UnitTestsOf<InfluxPoint>
    {
        [Test]
        public void Contructing_Should_throw_When_no_measurement_is_specified()
        {
            Action constructiong = () => SUT = new InfluxPoint(null);

            constructiong.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Contructing_Should_throw_When_empty_measurement_is_specified()
        {
            Action constructiong = () => SUT = new InfluxPoint(string.Empty);

            constructiong.ShouldThrow<ArgumentException>();
        }

        [Test]
        public void Constructing_Should_escape_measurement_name_When_name_contains_spaces()
        {
            SUT = new InfluxPoint("test of this");

            SUT.Measurement.Should().Be("test\\ of\\ this");
        }

        [Test]
        public void Constructing_Should_escape_measurement_name_When_name_contains_commas()
        {
            SUT = new InfluxPoint("test,of,this");

            SUT.Measurement.Should().Be("test\\,of\\,this");
        }

        [Test]
        public void IsComplete_Should_return_false_When_only_measurement_is_provided()
        {
            SUT = CreatePoint();

            SUT.IsComplete().Should().BeFalse();
        }

        [Test]
        public void IsComplete_Should_return_false_When_measurement_and_tag_is_provided()
        {
            SUT = CreatePoint()
                .AddTag("Context", "Tests");

            SUT.IsComplete().Should().BeFalse();
        }

        [Test]
        public void IsComplete_Should_return_true_When_measurement_and_a_field_is_specified()
        {
            SUT = CreatePoint()
                .AddField("Field1", 1);

            SUT.IsComplete().Should().BeTrue();
        }

        [Test]
        public void AddTag_Should_add_string_value_When_string_is_passed()
        {
            SUT = CreatePoint()
                .AddTag("Test", "mystring");

            SUT.ShouldContainTag("Test", "mystring");
        }

        [Test]
        public void AddTag_Should_escape_name_When_name_contains_spaces()
        {
            SUT = CreatePoint()
                .AddTag("Test of string", "mystring");

            SUT.ShouldContainTag("Test\\ of\\ string", "mystring");
        }

        [Test]
        public void AddTag_Should_add_escaped_string_value_When_string_value_with_spaces_are_passed()
        {
            SUT = CreatePoint()
                .AddTag("Test", "the string is");

            SUT.ShouldContainTag("Test", "the\\ string\\ is");
        }

        [Test]
        public void AddTag_Should_add_escaped_string_value_When_string_value_with_commas_are_passed()
        {
            SUT = CreatePoint()
                .AddTag("Test", "funny,string,is");

            SUT.ShouldContainTag("Test", "funny\\,string\\,is");
        }

        [Test]
        public void AddField_Should_add_string_value_When_string_is_passed()
        {
            SUT = CreatePoint()
                .AddField("Test", "mystring");

            SUT.ShouldContainField("Test", "mystring");
        }

        [Test]
        public void AddField_Should_escape_name_When_name_contains_spaces()
        {
            SUT = CreatePoint()
                .AddField("Test of string", "mystring");

            SUT.ShouldContainField("Test\\ of\\ string", "mystring");
        }

        [Test]
        public void AddField_Should_add_escaped_string_value_When_string_value_with_spaces_are_passed()
        {
            SUT = CreatePoint()
                .AddField("Test", "the string is");

            SUT.ShouldContainField("Test", "the\\ string\\ is");
        }

        [Test]
        public void AddField_Should_add_escaped_string_value_When_string_value_with_commas_are_passed()
        {
            SUT = CreatePoint()
                .AddField("Test", "funny,string,is");

            SUT.ShouldContainField("Test", "funny\\,string\\,is");
        }

        private InfluxPoint CreatePoint()
        {
            return SUT = new InfluxPoint("Test");
        }
    }
}