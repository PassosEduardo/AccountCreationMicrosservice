using AccountService.Infrastructure.Kafka;
using Confluent.Kafka;
using Microsoft.Extensions.Options;

namespace AccountService.Services.Kafka;

public class KafkaService : IKafkaService
{
    private readonly IKafkaInfrastructure _kafkaInfrastructure;

    public KafkaService(IOptions<KafkaInfrastructure> kafkaInfrastructure)
    {
        _kafkaInfrastructure = kafkaInfrastructure.Value;
    }
    public async Task PublishEmailConfirmationEventAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaInfrastructure.BootstrapServer
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var topic = _kafkaInfrastructure.Topics.GetValueOrDefault("EmailConfirmation");

            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentNullException("Topic EmailConfirmation not found");

            var deliveryReport = await producer.ProduceAsync(
                topic,
                new Message<Null, string> { Value = message });
        }
    }

    public async Task PublishPasswordResetEventAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = _kafkaInfrastructure.BootstrapServer
        };

        using (var producer = new ProducerBuilder<Null, string>(config).Build())
        {
            var topic = _kafkaInfrastructure.Topics.GetValueOrDefault("PasswordReset");

            if (string.IsNullOrWhiteSpace(topic))
                throw new ArgumentNullException("Topic PasswordReset not found");

            var deliveryReport = await producer.ProduceAsync(
                topic,
                new Message<Null, string> { Value = message });
        }
    }
}
