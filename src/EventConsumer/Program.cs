using EventConsumer.Configuration;
using EventConsumer.Consumers;
using System.Diagnostics.Eventing.Reader;
using System.Runtime.CompilerServices;

namespace EventConsumer;

public class Program
{
    public static void Main(string[] args)
    {
        CreateHostBuilder(args).Build().Run();
    }

    static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostContext, build) =>
            {
                Console.WriteLine("Starting application...");

                var hostEnviroment = hostContext.HostingEnvironment;
                var env = hostEnviroment.EnvironmentName;

                Console.WriteLine($"{hostEnviroment.GetType().Name}: {env}");

                if(!string.Equals("Production", env))
                    build.AddJsonFile($"appSettings.{env}.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureInfrastructure(hostContext.Configuration);
                services.AddHostedService<EmailConfirmationConsumer>();
                services.AddHostedService<PasswordResetConsumer>();
            });


}
