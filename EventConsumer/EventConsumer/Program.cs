using EventConsumer.Configuration;
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
                var hostEnviroment = hostContext.HostingEnvironment;
                var env = hostEnviroment.EnvironmentName;

                if(!string.Equals("Production", env))
                    build.AddJsonFile($"appSettings.{env}.json");
            })
            .ConfigureServices((hostContext, services) =>
            {
                services.ConfigureInfrastructure(hostContext.Configuration);
                services.AddHostedService<AccountServiceConsumer>();
            });


}
