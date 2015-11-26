using System;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using Requester;

namespace MyInfluxDbClient
{
    [Serializable]
    public class InfluxDbClientException : Exception
    {
        public HttpMethod HttpMethod { get; private set; }
        public HttpStatusCode HttpStatus { get; private set; }
        public Uri Uri { get; private set; }
        public string Reason { get; private set; }
        public string Content { get; private set; }

        internal InfluxDbClientException(HttpTextResponse response)
            : this(response.RequestMethod, response.StatusCode, response.RequestUri, response.Reason, response.Content)
        { }

        public InfluxDbClientException(HttpMethod httpMethod, HttpStatusCode httpStatus, Uri uri, string reason, string content)
            : base(string.Format("InfluxDb request failed.{0}HttpMethod: {1}{0}HttpStatus: {2}{0}Uri:{3}{0}Reason: {4}{0}Content: {5}{0}",
                Environment.NewLine,
                httpMethod,
                httpStatus,
                uri,
                reason,
                content))
        {
            HttpMethod = httpMethod;
            HttpStatus = httpStatus;
            Uri = uri;
            Reason = reason;
            Content = content;
        }

        protected InfluxDbClientException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        { }
    }
}