namespace EventConsumer.Infrasctructure.Kafka;

public interface IKafkaInfrastructure
{
    public string BootstrapServer { get; }
    public Dictionary<string, string> Topics { get; }
    public string GroupName { get; }
}
