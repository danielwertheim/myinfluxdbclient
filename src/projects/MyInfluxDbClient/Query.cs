using System.Text;
using EnsureThat;
using MyInfluxDbClient.Net;

namespace MyInfluxDbClient
{
    public abstract class Query<T> where T : Query<T>
    {
        protected string Command { get; }
        protected string FieldSelection { get; private set; }
        protected string From { get; private set; }
        protected string Where { get; private set; }

        protected Query(string command)
        {
            Ensure.That(command, nameof(command)).IsNotNullOrWhiteSpace();

            Command = command;
        }

        public T SelectedFields(string fields)
        {
            Ensure.That(fields, nameof(fields)).IsNotNullOrWhiteSpace();

            FieldSelection = fields;

            return this as T;
        }

        public T FromMeasurement(string measurement)
        {
            Ensure.That(measurement, nameof(measurement)).IsNotNullOrWhiteSpace();

            From = measurement;

            return this as T;
        }

        public T WhereTags(string predicate)
        {
            Ensure.That(predicate, nameof(predicate)).IsNotNullOrWhiteSpace();

            Where = predicate;

            return this as T;
        }

        public virtual string Generate()
        {
            var sb = new StringBuilder();

            sb.Append(Command);
            if (!string.IsNullOrWhiteSpace(FieldSelection))
            {
                sb.Append(" ");
                sb.Append(FieldSelection);
            }

            if (!string.IsNullOrWhiteSpace(From))
            {
                sb.Append(" from ");
                sb.Append("\"");
                sb.Append(UrlEncoder.Encode(From));
                sb.Append("\"");
            }

            if (!string.IsNullOrWhiteSpace(Where))
            {
                sb.Append(" where ");
                sb.Append(Where);
            }

            return sb.ToString();
        }
    }
}