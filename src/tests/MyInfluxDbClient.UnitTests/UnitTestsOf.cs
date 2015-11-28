using System;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public abstract class UnitTestsOf<TSut> : IDisposable
    {
        protected TSut SUT { get; set; }

        public void Dispose()
        {
            var sutAsDisposable = SUT as IDisposable;
            sutAsDisposable?.Dispose();
        }

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