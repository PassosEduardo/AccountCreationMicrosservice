
using Confluent.Kafka;
using EventConsumer.Entities;
using EventConsumer.Infrasctructure.Kafka;
using EventConsumer.Infrasctructure.SMTP;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace EventConsumer.Consumers;

public class PasswordResetConsumer : BackgroundService
{
    private readonly IKafkaInfrastructure _kafkaInfrastructure;
    private readonly ISmtpInfrastructure _smtpInfrastructure;

    public PasswordResetConsumer(IOptions<KafkaInfrastructure> optKafka,
                                  IOptions<SmtpInfrastructure> optSmtp)
    {
        _kafkaInfrastructure = optKafka.Value;
        _smtpInfrastructure = optSmtp.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var topic = _kafkaInfrastructure.Topics.GetValueOrDefault("PasswordReset");

        if (string.IsNullOrWhiteSpace(topic))
            throw new ArgumentNullException($"PasswordReset topic not found");

        while (!stoppingToken.IsCancellationRequested)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaInfrastructure.BootstrapServer,
                GroupId = _kafkaInfrastructure.GroupName,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(topic);
            CancellationTokenSource cts = new CancellationTokenSource();
            Console.CancelKeyPress += (_, e) =>
            {
                e.Cancel = true;
                cts.Cancel();
            };

            try
            {
                while (true)
                {
                    var consumerResult = consumer.Consume(cts.Token);
                    ResetPasswordEvent eventValue = JsonSerializer.Deserialize<ResetPasswordEvent>(consumerResult.Message.Value);

                    Console.WriteLine($"Sending email to: {eventValue.Email}");

                    await _smtpInfrastructure.SendPasswordResetAsync(eventValue);
                }
            }
            catch (Exception ex)
            {
                consumer.Close();
            }
        }
    }
}
