using System;
using System.Diagnostics;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    public abstract class IntegrationTestsOf<TSut> : IDisposable
    {
        protected TSut SUT { get; set; }

        protected IntegrationTestsOf() { }

        [DebuggerStepThrough]
        public void Dispose()
        {
            var sutAsDisposable = SUT as IDisposable;
            sutAsDisposable?.Dispose();
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