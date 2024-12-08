namespace AccountService.Infrastructure.Kafka;

public class KafkaInfrastructure : IKafkaInfrastructure
{
    public string BootstrapServer { get; set; }
    public Dictionary<string, string> Topics { get; set; } = new();
}
