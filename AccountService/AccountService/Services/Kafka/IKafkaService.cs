using System.Net;
using System.Text.Json;

namespace AccountService.Services.Kafka;

public interface IKafkaService
{
    Task PublishEmailConfirmationEventAsync(string message);
    Task PublishPasswordResetEventAsync(string message);
}
