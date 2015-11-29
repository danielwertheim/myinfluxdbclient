using System;
using System.Diagnostics;
using NUnit.Framework;

namespace MyInfluxDbClient.UnitTests
{
    public abstract class UnitTestsOf<TSut> : UnitTests, IDisposable
    {
        protected TSut SUT { get; set; }

        [DebuggerStepThrough]
        public void Dispose()
        {
            var sutAsDisposable = SUT as IDisposable;
            sutAsDisposable?.Dispose();
        }
    }

    public abstract class UnitTests
    {
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