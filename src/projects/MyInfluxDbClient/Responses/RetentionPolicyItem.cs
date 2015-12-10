namespace MyInfluxDbClient.Responses
{
    public class RetentionPolicyItem
    {
        public string Name { get; set; }
        public string Duration { get; set; }
        public int ReplicaN { get; set; }
        public bool IsDefault { get; set; }
    }
}