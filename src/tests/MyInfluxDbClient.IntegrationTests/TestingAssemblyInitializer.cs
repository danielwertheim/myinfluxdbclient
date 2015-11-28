using System;
using NUnit.Framework;

namespace MyInfluxDbClient.IntegrationTests
{
    [SetUpFixture]
    public class TestingAssemblyInitializer : IDisposable
    {
        public TestingAssemblyInitializer()
        {
            //CREATE DB
        }

        public void Dispose()
        {
            //REMOVE DB
        }
    }
}