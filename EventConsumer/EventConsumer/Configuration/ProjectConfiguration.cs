using EventConsumer.Infrasctructure.Kafka;
using EventConsumer.Infrasctructure.SMTP;

namespace EventConsumer.Configuration;

public static class ProjectConfiguration
{
    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services,
                                                             IConfiguration configuration)
    {
        services.Configure<KafkaInfrastructure>(configuration.GetSection(nameof(KafkaInfrastructure)));
        services.Configure<SmtpInfrastructure>(configuration.GetSection(nameof(SmtpInfrastructure)));

        services.AddScoped<IKafkaInfrastructure, KafkaInfrastructure>();
        services.AddScoped<ISmtpInfrastructure, SmtpInfrastructure>();

        return services;
    }
}
