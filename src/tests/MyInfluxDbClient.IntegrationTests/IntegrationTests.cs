using System;
using System.Diagnostics;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public abstract class IntegrationTests : IDisposable
    {
        protected InfluxDbClient Client { get; set; }

        protected IntegrationTests()
        {
            Client = new InfluxDbClient(IntegrationTestsRuntime.Host);
        }

        [DebuggerStepThrough]
        public void Dispose()
        {
            Client?.Dispose();
            Client = null;
        }

        [SetUp]
        [DebuggerStepThrough]
        protected virtual void OnBeforeEachTest() { }

        [TearDown]
        [DebuggerStepThrough]
        protected virtual void OnAfterEachTest() { }

        [OneTimeSetUp]
        [DebuggerStepThrough]
        protected virtual void OnBeforeAllTests() { }

        [OneTimeTearDown]
        [DebuggerStepThrough]
        protected virtual void OnAfterAllTests() { }
    }
}