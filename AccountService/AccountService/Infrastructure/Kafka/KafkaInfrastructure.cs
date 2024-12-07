namespace AccountService.Infrastructure.Kafka;

public class KafkaInfrastructure : IKafkaInfrastructure
{
    public string BootstrapServer { get; set; }
    public string TopicName { get; set; }
}
