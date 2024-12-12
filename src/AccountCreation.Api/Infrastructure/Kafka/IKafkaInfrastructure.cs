namespace AccountService.Infrastructure.Kafka;

public interface IKafkaInfrastructure
{
    string BootstrapServer { get; set; }
    Dictionary<string, string> Topics { get; set; }
}
