namespace EventConsumer.Infrasctructure.Kafka;

public class KafkaInfrastructure : IKafkaInfrastructure
{
    public string BootstrapServer { get; set; }
    public Dictionary<string,string> Topics { get; set; }
    public string GroupName { get; set; }
}
