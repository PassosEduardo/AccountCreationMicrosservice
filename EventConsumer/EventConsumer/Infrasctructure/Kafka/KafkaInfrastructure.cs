namespace EventConsumer.Infrasctructure.Kafka;

public class KafkaInfrastructure : IKafkaInfrastructure
{
    public string BootstrapServer { get; set; }
    public string TopicName { get; set; }
    public string GroupName { get; set; }
}
