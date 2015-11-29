using System;
using System.Collections.Generic;
using FluentAssertions;
using MyInfluxDbClient.UnitTests.TestData;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public class InfluxPointsUnitTests : UnitTestsOf<InfluxPoints>
    {
        protected override void OnBeforeEachTest()
        {
            SUT = new InfluxPoints();
        }

        [Test]
        public void Count_Should_be_zero_When_just_constructed()
        {
            SUT.Count.Should().Be(0);
        }

        [Test]
        public void IsEmpty_Should_be_true_When_just_constructed()
        {
            SUT.IsEmpty.Should().BeTrue();
        }

        [Test]
        public void ShouldValidatePoints_Should_be_true_When_just_constructed()
        {
            SUT.ShouldValidatePoints.Should().BeTrue();
        }

        [Test]
        public void DisablePointValidatoin_Should_make_ShouldValidatePoints_become_false()
        {
            SUT.DisablePointValidation();

            SUT.ShouldValidatePoints.Should().BeFalse();
        }

        [Test]
        public void Add_of_single_point_Should_increase_Count()
        {
            SUT.Add(InfluxPointsTestData.Instance.CreateSimplest());

            SUT.Count.Should().Be(1);
        }

        [Test]
        public void Add_of_single_point_Should_make_IsEmpty_false()
        {
            SUT.Add(InfluxPointsTestData.Instance.CreateSimplest());

            SUT.IsEmpty.Should().BeFalse();
        }

        [Test]
        public void Add_of_single_point_Should_add_the_point()
        {
            var point = InfluxPointsTestData.Instance.CreateSimplest();

            SUT.Add(point);

            SUT.Should().Contain(point);
            SUT[0].Should().Be(point);
        }

        [Test]
        public void Add_of_single_point_Should_throw_When_point_is_incomplete()
        {
            var incompletePoint = InfluxPointsTestData.Instance.CreateIncomplete();

            SUT
                .Invoking(i => i.Add(incompletePoint))
                .ShouldThrow<ArgumentException>()
                .Where(ex => ex.ParamName == "point" && ex.Message.Contains("Point is not valid. Ensure minimum information exists (measurement and one field value)."));
            SUT.Should().NotContain(incompletePoint);
        }

        [Test]
        public void Add_of_many_points_Should_increase_Count()
        {
            const int numOfPoints = 3;

            SUT.Add(InfluxPointsTestData.Instance.CreateMany(numOfPoints, fn => fn.CreateSimplest));

            SUT.Count.Should().Be(3);
        }

        [Test]
        public void Add_of_many_points_Should_make_IsEmpty_false()
        {
            SUT.Add(InfluxPointsTestData.Instance.CreateMany(3, fn => fn.CreateSimplest));

            SUT.IsEmpty.Should().BeFalse();
        }

        [Test]
        public void Add_of_many_points_Should_add_the_points()
        {
            var points = InfluxPointsTestData.Instance.CreateMany(3, fn => fn.CreateSimplest);

            SUT.Add(points);

            SUT.Should().Contain(points);
        }

        [Test]
        public void Add_of_many_points_Should_add_first_but_throw_on_second_When_first_is_OK_and_second_point_is_incomplete()
        {
            var points = new List<InfluxPoint>
            {
                InfluxPointsTestData.Instance.CreateSimplest(),
                InfluxPointsTestData.Instance.CreateIncomplete()
            };

            SUT
                .Invoking(i => i.Add(points))
                .ShouldThrow<ArgumentException>()
                .Where(ex => ex.ParamName == "point" && ex.Message.Contains("Point is not valid. Ensure minimum information exists (measurement and one field value)."));

            SUT.Should().Contain(points[0]);
            SUT.Should().NotContain(points[1]);
        }

        [Test]
        public void Add_of_single_point_Should_not_throw_When_point_is_incomplete_and_validation_has_been_turned_of()
        {
            var incompletePoint = InfluxPointsTestData.Instance.CreateIncomplete();
            SUT.DisablePointValidation();

            SUT.Add(incompletePoint);

            SUT.Should().Contain(incompletePoint);
        }

        [Test]
        public void Add_of_many_points_Should_not_throw_When_points_are_incomplete_and_validation_has_been_turned_of()
        {
            var incompletePoints = InfluxPointsTestData.Instance.CreateMany(3, i => i.CreateIncomplete);
            SUT.DisablePointValidation();

            SUT.Add(incompletePoints);

            SUT.Should().Contain(incompletePoints);
        }
    }
}