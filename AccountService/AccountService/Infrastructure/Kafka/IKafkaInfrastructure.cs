namespace AccountService.Infrastructure.Kafka;

public interface IKafkaInfrastructure
{
    string BootstrapServer { get; set; }
    string TopicName { get; set; }
}
