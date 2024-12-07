using AccountService.Infrastructure.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace AccountService.Services.Kafka;

public interface IKafkaService
{
    Task PublishMessageToTopicAsync(string email);
}

public class KafkaService : IKafkaService
{
    private readonly IKafkaInfrastructure _kafkaInfrastructure;

    public KafkaService(IOptions<KafkaInfrastructure> kafkaInfrastructure)
    {
        _kafkaInfrastructure = kafkaInfrastructure.Value;
    }
    public async Task PublishMessageToTopicAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaInfrastructure.BootstrapServer
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var deliveryReport = await producer.ProduceAsync(
                _kafkaInfrastructure.TopicName,
                new Message<Null, string> { Value = message });
        }
    }
}
