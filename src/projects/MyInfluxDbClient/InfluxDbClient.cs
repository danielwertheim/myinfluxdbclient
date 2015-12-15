using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EnsureThat;
using MyInfluxDbClient.Extensions;
using MyInfluxDbClient.Meta;
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

            var request = CreateCommandRequest($"create database {UrlEncoder.Encode(databaseName)}");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task DropDatabaseAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var request = CreateCommandRequest($"drop database {UrlEncoder.Encode(databaseName)}");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
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

            var json = await GetDatabaseNamesJsonAsync().ForAwait();
            var data = Requester.JsonSerializer.Deserialize<InfluxDbResponse>(json);

            return data.Results.SingleOrDefault()?.Series.SingleOrDefault()?.Values.Select(v => v.First.ToString()).ToArray();
        }

        public async Task<string> GetDatabaseNamesJsonAsync()
        {
            ThrowIfDisposed();

            var request = CreateCommandRequest("show databases");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulRead(response);

            return response.Content;
        }

        public async Task CreateRetentionPolicyAsync(string databaseName, CreateRetentionPolicy policy)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(policy, nameof(policy)).IsNotNull();

            var defaultString = policy.MakeDefault.HasValue && policy.MakeDefault.Value ? " default" : string.Empty;
            var request = CreateCommandRequest($"create retention policy {UrlEncoder.Encode(policy.Name)} on {UrlEncoder.Encode(databaseName)} duration {policy.Duration} replication {policy.Replication}{defaultString}");

            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task AlterRetentionPolicyAsync(string databaseName, AlterRetentionPolicy policy)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(policy, nameof(policy)).IsNotNull();

            var cmd = new StringBuilder();
            cmd.Append($"alter retention policy {UrlEncoder.Encode(policy.Name)} on {UrlEncoder.Encode(databaseName)}");
            if (policy.Duration != null)
                cmd.Append($" duration {policy.Duration}");
            if(policy.Replication.HasValue)
                cmd.Append($" replication {policy.Replication.Value}");
            if (policy.MakeDefault.HasValue && policy.MakeDefault.Value)
                cmd.Append(" default");

            var request = CreateCommandRequest(cmd.ToString());
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task DropRetentionPolicyAsync(string databaseName, string policyName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(policyName, nameof(policyName)).IsNotNullOrWhiteSpace();

            var request = CreateCommandRequest($"drop retention policy {UrlEncoder.Encode(policyName)} on {UrlEncoder.Encode(databaseName)}");

            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task<RetentionPolicyItem[]> GetRetentionPoliciesAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var json = await GetRetentionPoliciesJsonAsync(databaseName).ForAwait();
            var data = Requester.JsonSerializer.Deserialize<InfluxDbResponse>(json);
            var serie = data.Results.SingleOrDefault()?.Series.SingleOrDefault();
            if (serie == null || !serie.Values.Any())
                return new RetentionPolicyItem[0];

            var schema = serie.GetSchemaOrdinals();
            return serie.Values.Select(value => new RetentionPolicyItem
            {
                Name = value[schema[RetentionPolicySchema.Name]].ToObject<string>(),
                Duration = value[schema[RetentionPolicySchema.Duration]].ToObject<string>(),
                ReplicaN = value[schema[RetentionPolicySchema.ReplicaN]].ToObject<int>(),
                IsDefault = value[schema[RetentionPolicySchema.IsDefault]].ToObject<bool>()
            }).ToArray();
        }

        public async Task<string> GetRetentionPoliciesJsonAsync(string databaseName)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var request = CreateCommandRequest($"show retention policies on {UrlEncoder.Encode(databaseName)}");
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulRead(response);

            return response.Content;
        }

        public async Task DropSeriesAsync(string databaseName, DropSeries query)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(query, nameof(query)).IsNotNull();

            var request = CreateCommandRequest(query.Generate(), databaseName);
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        public async Task<Series> GetSeriesAsync(string databaseName, GetSeries query = null)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            var result = new Series();

            var json = await GetSeriesJsonAsync(databaseName, query).ForAwait();
            var data = Requester.JsonSerializer.Deserialize<InfluxDbResponse>(json);
            if (data?.Results == null || !data.Results.Any())
                return result;

            foreach (var serie in data.Results.SelectMany(r => r.Series))
            {
                var schema = serie.GetSchemaOrdinals();
                var keyOrdinal = schema[SerieSchema.Key];
                result.Add(serie.Name, serie.Values.Select(value =>
                {
                    var serieItem = new SerieItem
                    {
                        Key = value[keyOrdinal].ToObject<string>()
                    };

                    for (var ci = 0; ci < serie.Columns.Count; ci++)
                    {
                        if(ci == keyOrdinal)
                            continue;

                        serieItem.Tags.Add(serie.Columns[ci], value[ci].ToObject<string>());
                    }

                    return serieItem;
                }).ToArray());
            }

            return result;
        }

        public async Task<string> GetSeriesJsonAsync(string databaseName, GetSeries query = null)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();

            query = query ?? new GetSeries();

            var request = CreateCommandRequest(query.Generate(), databaseName);
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessfulRead(response);

            return response.Content;
        }

        public async Task WriteAsync(string databaseName, InfluxPoints points, WriteOptions options = null)
        {
            ThrowIfDisposed();

            Ensure.That(databaseName, nameof(databaseName)).IsNotNullOrWhiteSpace();
            Ensure.That(points, nameof(points)).IsNotNull();
            Ensure.That(points.IsEmpty, nameof(points)).WithExtraMessageOf(() => "Can not write empty points collections.").IsFalse();

            var request = CreateWritePointsRequest(databaseName, points, options);
            var response = await Requester.SendAsync(request).ForAwait();
            EnsureSuccessful(response);
        }

        protected virtual HttpRequest CreateCommandRequest(string command)
        {
            return new HttpRequest(HttpMethod.Get, $"query?q={command}");
        }

        protected virtual HttpRequest CreateCommandRequest(string command, string dbName)
        {
            return new HttpRequest(HttpMethod.Get, $"query?db={UrlEncoder.Encode(dbName)}&q={command}");
        }

        protected virtual HttpRequest CreateWritePointsRequest(string databaseName, InfluxPoints points, WriteOptions options = null)
        {
            var bytesContent = GetBytesContent(points);
            var writeOptionUrlArgs = WriteOptionsUrlArgsBuilder.Build(options ?? DefaultWriteOptions);

            return new HttpRequest(HttpMethod.Post, $"write?db={UrlEncoder.Encode(databaseName)}{writeOptionUrlArgs}")
                .WithContent(bytesContent);
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

        protected virtual void EnsureSuccessful(HttpTextResponse response)
        {
            if (!response.IsSuccess)
                throw new InfluxDbClientException(response);

            //InfluxDB reports 204 for awesomeness and 200 for "I understood the request, but something might have failed".
            if (response.StatusCode == HttpStatusCode.NoContent)
                return;

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