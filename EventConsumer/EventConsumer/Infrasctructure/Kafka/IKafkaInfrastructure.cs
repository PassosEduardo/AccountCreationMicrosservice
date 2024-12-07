namespace EventConsumer.Infrasctructure.Kafka;

public interface IKafkaInfrastructure
{
    public string BootstrapServer { get; }
    public string TopicName { get; }
    public string GroupName { get; }
}
