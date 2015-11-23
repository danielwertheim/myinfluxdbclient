using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using Requester;

namespace InfluxDbClient
{
    public class InfluxDbClient : IDisposable
    {
        private HttpRequester _requester;

        protected bool IsDisposed { get; private set; }

        public InfluxDbClient(string serverUrl)
        {
            Ensure.That(serverUrl, nameof(serverUrl)).IsNotNullOrWhiteSpace();

            _requester = new HttpRequester(serverUrl);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            IsDisposed = true;
        }

        protected virtual void Dispose(bool disposing)
        {
            if (IsDisposed || !disposing)
                return;

            _requester?.Dispose();
            _requester = null;
        }

        protected virtual void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public Task<HttpTextResponse> CreateDbAsync(string dbName)
        {
            Ensure.That(dbName, nameof(dbName)).IsNotNullOrWhiteSpace();

            var request = new HttpRequest(HttpMethod.Get, $"query?q=create database {dbName}");

            return _requester.SendAsync(request);
        }

        public Task<HttpTextResponse> WriteMeasurementsAsync(string dbName, Measurements measurements)
        {
            Ensure.That(dbName, nameof(dbName)).IsNotNullOrWhiteSpace();
            Ensure.That(measurements, nameof(measurements)).IsNotNull();

            var request = new HttpRequest(HttpMethod.Post, $"write?db={dbName}")
                .WithContent(measurements.ToBytesContent());

            return _requester.SendAsync(request);
        }
    }
}