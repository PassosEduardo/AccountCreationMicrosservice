using AccountService.Infrastructure.Kafka;
using AccountService.Infrastructure.MongoInfrastrcuture;
using AccountService.Services.Account;
using AccountService.Services.Kafka;

namespace AccountService.Configuration;

public static class StartupConfigurations
{
    public static IServiceCollection ConfigureAppServices(this IServiceCollection services)
    {
        services.AddScoped<IAccountService, Services.Account.AccountService>();
        services.AddScoped<IKafkaService, KafkaService>();

        return services;
    }

    public static IServiceCollection ConfigureInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IKafkaInfrastructure, KafkaInfrastructure>();
        services.Configure<KafkaInfrastructure>(configuration.GetSection(nameof(KafkaInfrastructure)));
        services.AddScoped<IMongoRepository, MongoRepository>();

        return services;
    }
}
