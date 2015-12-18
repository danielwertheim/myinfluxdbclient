using EnsureThat;

namespace MyInfluxDbClient
{
    public class AlterRetentionPolicy
    {
        public string Name { get; }
        public RetentionPolicyDuration Duration { get; set; }
        public int? Replication { get; set; }
        public bool? MakeDefault { get; set; }

        public AlterRetentionPolicy(string name)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();

            Name = name;
        }
    }
}