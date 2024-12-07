
using Confluent.Kafka;
using EventConsumer.Entities;
using EventConsumer.Infrasctructure.Kafka;
using EventConsumer.Infrasctructure.SMTP;
using Microsoft.Extensions.Options;
using System.Net;
using System.Text.Json;

namespace EventConsumer;

public class AccountServiceConsumer : BackgroundService
{
    private readonly IKafkaInfrastructure _kafkaInfrastructure;
    private readonly ISmtpInfrastructure _smtpInfrastructure;

    public AccountServiceConsumer(IOptions<KafkaInfrastructure> optKafka, 
                                  IOptions<SmtpInfrastructure> optSmtp)
    {
        _kafkaInfrastructure = optKafka.Value;
        _smtpInfrastructure = optSmtp.Value;
    }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Console.WriteLine("Starting application...");

        while(!stoppingToken.IsCancellationRequested)
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = _kafkaInfrastructure.BootstrapServer,
                GroupId = _kafkaInfrastructure.GroupName,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();

            consumer.Subscribe(_kafkaInfrastructure.TopicName);
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
                    AccountEntity credentials = JsonSerializer.Deserialize<AccountEntity>(consumerResult.Message.Value);

                    Console.WriteLine($"Sending email to: {credentials.Email}");

                    await _smtpInfrastructure.SendEmailAsync(credentials);
                }
            }
            catch (Exception ex)
            {
                consumer.Close();
            }
        }
    }
}
