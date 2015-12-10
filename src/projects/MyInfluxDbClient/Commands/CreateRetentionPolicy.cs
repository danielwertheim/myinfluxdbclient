using EnsureThat;

namespace MyInfluxDbClient.Commands
{
    public class CreateRetentionPolicy
    {
        public string Name { get; }
        public RetentionPolicyDuration Duration { get; }
        public int Replication { get; }
        public bool? MakeDefault { get; }

        public CreateRetentionPolicy(string name, RetentionPolicyDuration duration, int replication, bool? makeDefault = null)
        {
            Ensure.That(name, nameof(name)).IsNotNullOrWhiteSpace();
            Ensure.That(duration, nameof(duration)).IsNotNull();
            Ensure.That(replication, nameof(replication)).IsGt(0);

            Name = name;
            Duration = duration;
            Replication = replication;
            MakeDefault = makeDefault;
        }
    }
}