using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using EnsureThat;
using MyInfluxDbClient.Extensions;
using MyInfluxDbClient.Net;
using MyInfluxDbClient.Protocols;
using MyInfluxDbClient.Responses;
using Requester;
using Requester.Http;

namespace MyInfluxDbClient
{
    public class InfluxDbClient : IDisposable, IInfluxDbClient
    {
        protected bool IsDisposed { get; private set; }
        protected HttpRequester Requester { get; private set; }
        protected IInfluxPointsSerializer InfluxPointsSerializer { get; set; }
        protected IWriteOptionsUrlArgsBuilder WriteOptionsUrlArgsBuilder { get; set; }

        public WriteOptions DefaultWriteOptions { get; }

        public InfluxDbClient(string host)
        {
            Ensure.That(host, nameof(host)).IsNotNullOrWhiteSpace();

            Requester = new HttpRequester(host);
            InfluxPointsSerializer = new LineProtocolInfluxPointsSerializer();
            DefaultWriteOptions = CreateDefaultWriteOptions();
            WriteOptionsUrlArgsBuilder = new WriteOptionsUrlArgsBuilder();
        }

        private static WriteOptions CreateDefaultWriteOptions()
        {
            return new WriteOptions();
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

            Requester?.Dispose();
            Requester = null;
        }

        protected void ThrowIfDisposed()
        {
            if (IsDisposed)
                throw new ObjectDisposedException(GetType().Name);
        }

        public async Task CreateDatabaseAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var request = CreateQRequest($"create database {UrlEncoder.Encode(databaseName)}");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulDbOp(response);
        }

        public async Task DropDatabaseAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var request = CreateQRequest($"drop database {UrlEncoder.Encode(databaseName)}");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulDbOp(response);
        }

        public async Task<bool> DatabaseExistsAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var databaseNames = await GetDatabaseNamesAsync().ForAwait();

            return databaseNames.Contains(databaseName);
        }

        public async Task<string[]> GetDatabaseNamesAsync()
        {
            ThrowIfDisposed();

            var request = CreateQRequest("show databases");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulRead(response);

            var data = Requester.JsonSerializer.Deserialize<InfluxDbResponse>(response.Content);

            return data.Results.SingleOrDefault()?.Series.SingleOrDefault()?.Values.Select(v => v.First.ToString()).ToArray();
        }

        public async Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(points, nameof(points)).IsNotNull();
            Ensure.That(points.IsEmpty, nameof(points)).WithExtraMessageOf(() => "Can not write empty points collections.").IsFalse();

            var request = CreateWritePointsRequest(databaseName, points, options);
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulWrite(response);
        }

        protected virtual HttpRequest CreateWritePointsRequest(string databaseName, InfluxPoints points, WriteOptions options = null)
        {
            var bytesContent = GetBytesContent(points);
            var writeOptionUrlArgs = WriteOptionsUrlArgsBuilder.Build(options ?? DefaultWriteOptions);

            return new HttpRequest(HttpMethod.Post, $"write?db={UrlEncoder.Encode(databaseName)}{writeOptionUrlArgs}")
                .WithContent(bytesContent);
        }

        protected virtual HttpRequest CreateQRequest(string command)
        {
            return new HttpRequest(HttpMethod.Get, $"query?q={command}");
        }

        protected virtual BytesContent GetBytesContent(InfluxPoints points)
        {
            var buff = InfluxPointsSerializer.Serialize(points);

            return new BytesContent(buff, HttpContentTypes.Instance.Text);
        }

        protected virtual void EnsureSuccessfulRead(HttpTextResponse response)
        {
            if (!response.IsSuccess)
                throw new InfluxDbClientException(response);
        }

        protected virtual void EnsureSuccessfulWrite(HttpTextResponse response)
        {
            if (!response.IsSuccess)
                throw new InfluxDbClientException(response);

            //InfluxDB reports 204 for awesomeness and 200 for "I understood the request, but something failed".
            if (response.StatusCode == HttpStatusCode.NoContent)
                return;

            var errorResult = GetErrorResult(response);
            if (!string.IsNullOrWhiteSpace(errorResult?.Error))
                throw new InfluxDbClientException(response, errorResult.Error);
        }

        protected virtual void EnsureSuccessfulDbOp(HttpTextResponse response)
        {
            if (!response.IsSuccess)
                throw new InfluxDbClientException(response);

            if (response.StatusCode == HttpStatusCode.NoContent)
                return;

            //InfluxDB reports 200 even if result contains error object
            var errorResult = GetErrorResult(response);
            if (!string.IsNullOrWhiteSpace(errorResult?.Error))
                throw new InfluxDbClientException(response, errorResult.Error);
        }

        private ErrorResult GetErrorResult(HttpTextResponse response)
        {
            var errorResponse = Requester.JsonSerializer.Deserialize<InfluxDbErrorResponse>(response.Content);
            var errorResult = errorResponse?.Results.FirstOrDefault();
            return errorResult;
        }
    }
}