using System.Text;
using EnsureThat;

namespace MyInfluxDbClient
{
    public abstract class SeriesQuery : IQuery
    {
        public string Command { get; }
        public string From { get; private set; }
        public string Where { get; private set; }

        protected SeriesQuery(string command)
        {
            Ensure.That(command, nameof(command)).IsNotNullOrWhiteSpace();

            Command = command;
        }

        public SeriesQuery FromMeasurement(string measurement)
        {
            Ensure.That(measurement, nameof(measurement)).IsNotNullOrWhiteSpace();

            From = measurement;

            return this;
        }

        public SeriesQuery WhereTags(string tagPredicate)
        {
            Ensure.That(tagPredicate, nameof(tagPredicate)).IsNotNullOrWhiteSpace();

            Where = tagPredicate;

            return this;
        }

        public string Generate()
        {
            var sb = new StringBuilder();

            sb.Append(Command);
            if (!string.IsNullOrWhiteSpace(From))
            {
                sb.Append(" from ");
                sb.Append(From);
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