using PetroineosIntradayReport.Core;
using PetroineosIntradayReport.Service;
using Services;

var host = Host.CreateDefaultBuilder(args)
    .UseWindowsService(options =>
    {
        options.ServiceName = "Petroineos Intraday Report Service";
    })
    .ConfigureServices((hostContext, services) =>
    {
        var configuration = hostContext.Configuration;
        services.Configure<Configuration>(configuration.GetSection(nameof(Configuration)));
        services.AddSingleton<IPowerService,PowerService>();
        services.AddSingleton<TradePositionsStore>();
        services.AddSingleton<TradePositionsAggregator>();
        services.AddSingleton<IntradayReportService>();
        services.AddHostedService<WindowsService>();
    })
    .ConfigureLogging((context, logging) =>
    {
        logging.AddConfiguration(
            context.Configuration.GetSection("Logging"));
    })
    .Build();

await host.RunAsync();
