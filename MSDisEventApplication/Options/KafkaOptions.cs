namespace MSDisEventApplication.Options
{
    public class KafkaOptions
    {
        public const string Name = "Kafka";
        public string BootstrapServers { get; set; } = string.Empty;
        public string GroupId { get; set; } = string.Empty;
        public string Topic { get; set; } = string.Empty;
    }
}
