using System;
using FluentAssertions;
using MyInfluxDbClient.Protocols;
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
        public void Constructing_Should_escape_measurement_name_When_name_contains_quotes()
        {
            SUT = new InfluxPoint("\"test\"");

            SUT.Measurement.Should().Be("\\\"test\\\"");
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
        public void HasTags_Should_return_false_When_no_tags_exists()
        {
            SUT.HasTags.Should().BeFalse();
        }

        [Test]
        public void HasTags_Should_return_true_When_tags_exists()
        {
            SUT.AddTag("test", "test");

            SUT.HasTags.Should().BeTrue();
        }

        [Test]
        public void HasFields_Should_return_false_When_no_fields_exists()
        {
            SUT.HasFields.Should().BeFalse();
        }

        [Test]
        public void HasFields_Should_return_true_When_fields_exists()
        {
            SUT.AddField("test", "test");

            SUT.HasFields.Should().BeTrue();
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
        public void AddTag_Should_add_escaped_string_value_When_string_value_with_quotes_are_passed()
        {
            SUT = CreatePoint()
                .AddTag("Test", "\"quoted\"");

            SUT.ShouldContainTag("Test", "\\\"quoted\\\"");
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

        [Test]
        public void AddField_Should_add_escaped_string_value_When_string_value_with_quotes_are_passed()
        {
            SUT = CreatePoint()
                .AddField("Test", "\"quoted\"");

            SUT.ShouldContainField("Test", "\\\"quoted\\\"");
        }

        [Test]
        public void AddTimeStamp_Should_set_to_now_with_default_resolution_When_nothing_is_specified()
        {
            var expexted = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            SUT = new InfluxPoint("Test");

            SUT.AddTimeStamp();

            SUT.TimeStamp.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
            SUT.TimeStampResolution.Should().Be(TimeStampResolutions.Default);
        }

        [Test]
        public void AddTimeStamp_Should_set_to_now_and_to_provided_resolution_When_only_resolution_is_specified()
        {
            var expexted = TimeSpan.FromTicks(DateTime.UtcNow.Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            SUT = new InfluxPoint("Test");

            SUT.AddTimeStamp(TimeStampResolutions.Seconds);

            SUT.TimeStamp.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
            SUT.TimeStampResolution.Should().Be(TimeStampResolutions.Seconds);
        }

        [Test]
        public void AddTimeStamp_Should_set_to_provided_values_When_time_stamp_and_resolution_is_specified()
        {
            var initial = new DateTime(2010, 1, 1, 23, 54, 11);
            var expexted = TimeSpan.FromTicks(initial.ToUniversalTime().Ticks - InfluxDbEnvironment.EpochTicks);
            var margin = expexted.Add(TimeSpan.FromSeconds(1));

            SUT = new InfluxPoint("Test");

            SUT.AddTimeStamp(initial, TimeStampResolutions.Seconds);

            SUT.TimeStamp.Ticks.Should().BeInRange(expexted.Ticks, margin.Ticks);
            SUT.TimeStampResolution.Should().Be(TimeStampResolutions.Seconds);
        }

        private InfluxPoint CreatePoint()
        {
            return SUT = new InfluxPoint("Test");
        }
    }
}