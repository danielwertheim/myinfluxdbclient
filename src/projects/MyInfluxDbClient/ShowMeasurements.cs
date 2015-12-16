using System.Text;
using EnsureThat;

namespace MyInfluxDbClient
{
    public class ShowMeasurements
    {
        public string Where { get; private set; }

        public ShowMeasurements WhereTags(string predicate)
        {
            Ensure.That(predicate, nameof(predicate)).IsNotNullOrWhiteSpace();

            Where = predicate;

            return this;
        }

        public virtual string Generate()
        {
            var sb = new StringBuilder();

            sb.Append("show measurements");

            if (!string.IsNullOrWhiteSpace(Where))
            {
                sb.Append(" where ");
                sb.Append(Where);
            }

            return sb.ToString();
        }
    }
}