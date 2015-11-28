using System;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public abstract class IntegrationTestsOf<TSut> : IDisposable
    {
        protected TSut SUT { get; set; }

        protected IntegrationTestsOf() { }

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