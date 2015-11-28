using System;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public abstract class UnitTestsOf<TSut> : UnitTests, IDisposable
    {
        protected TSut SUT { get; set; }

        public void Dispose()
        {
            var sutAsDisposable = SUT as IDisposable;
            sutAsDisposable?.Dispose();
        }
    }

    public abstract class UnitTests
    {
        [SetUp]
        protected virtual void OnBeforeEachTest() { }

        [TearDown]
        protected virtual void OnAfterEachTest() { }

        [OneTimeSetUp]
        protected virtual void OnBeforeAllTests() { }

        [OneTimeTearDown]
        protected virtual void OnAfterAllTests() { }
    }
}