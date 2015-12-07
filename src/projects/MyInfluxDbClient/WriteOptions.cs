using EnsureThat;

namespace MyInfluxDbClient
{
    public class WriteOptions
    {
        public string RetentionPolicy { get; private set; }
        public TimeStampPrecision? TimeStampPrecision { get; private set; }
        public Consistency? Consistency { get; private set; }

        public WriteOptions SetRetentionPolicy(string value)
        {
            Ensure.That(value).IsNotNullOrWhiteSpace();

            RetentionPolicy = value;

            return this;
        }

        public WriteOptions SetTimeStampPrecision(TimeStampPrecision value)
        {
            TimeStampPrecision = value;

            return this;
        }

        public WriteOptions SetConsistency(Consistency value)
        {
            Consistency = value;

            return this;
        }
    }
}