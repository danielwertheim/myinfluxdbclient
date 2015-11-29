using System;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyInfluxDbClient.Extensions;
using MyInfluxDbClient.Protocols;
using Requester;
using Requester.Http;

namespace MyInfluxDbClient
{
    public class InfluxDbClient : IDisposable
    {
        private HttpRequester _requester;

        protected bool IsDisposed { get; private set; }
        protected IInfluxPointsSerializer InfluxPointsSerializer { get; set; }

        public InfluxDbClient(string serverUrl)
        {
            Ensure.That(serverUrl, nameof(serverUrl)).IsNotNullOrWhiteSpace();

            _requester = new HttpRequester(serverUrl);
            InfluxPointsSerializer = new LineProtocolInfluxPointsSerializer();
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

        public async Task CreateDbAsync(string dbName)
        {
            Ensure.That(dbName, nameof(dbName)).IsNotNullOrWhiteSpace();

            var request = new HttpRequest(HttpMethod.Get, $"query?q=create database {dbName}");

            var response = await _requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task WriteAsync(string dbName, InfluxPoints points)
        {
            Ensure.That(dbName, nameof(dbName)).IsNotNullOrWhiteSpace();
            Ensure.That(points, nameof(points)).IsNotNull();
            Ensure.That(points.IsEmpty, nameof(points)).WithExtraMessageOf(() => "Can not write empty points collections.").IsFalse();

            var bytesContent = GetBytesContent(points);
            var request = new HttpRequest(HttpMethod.Post, $"write?db={dbName}")
                .WithContent(bytesContent);

            var response = await _requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        protected BytesContent GetBytesContent(InfluxPoints points)
        {
            var buff = InfluxPointsSerializer.Serialize(points);

            return new BytesContent(buff, HttpContentTypes.Instance.Text);
        }

        protected void EnsureSuccessful(HttpTextResponse response)
        {
            if (!response.IsSuccess)
                throw new InfluxDbClientException(response);
        }
    }
}