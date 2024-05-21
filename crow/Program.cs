using crow;
using crow.data;
using crow.src.jobs;

IHostBuilder builder = Host.CreateDefaultBuilder(args);

    builder.ConfigureServices(services =>
    {
        services
            .AddHostedService<Worker>()
            .AddSingleton<RabbitMQJob>()
            .AddSingleton<DapperContext>()
            .AddServices();
        
    });

IHost host = builder.Build(); 

await host.RunAsync();
