using System.Text;
using EnsureThat;

namespace MyInfluxDbClient.Net
{
    public class WriteOptionsUrlArgsBuilder : IWriteOptionsUrlArgsBuilder
    {
        public virtual string Build(WriteOptions options)
        {
            Ensure.That(options, nameof(options)).IsNotNull();

            var sb = new StringBuilder();

            if (!string.IsNullOrWhiteSpace(options.RetentionPolicy))
            {
                sb.Append("&rp=");
                sb.Append(options.RetentionPolicy);
            }

            if (options.Consistency.HasValue)
            {
                sb.Append("&consistency=");
                sb.Append(options.Consistency.Value.ToUrlValue());
            }

            if (options.TimeStampPrecision.HasValue)
            {
                sb.Append("&precision=");
                sb.Append(options.TimeStampPrecision.Value.ToUrlValue());
            }

            return sb.ToString();
        }
    }
}