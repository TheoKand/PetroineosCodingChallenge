using Microsoft.Extensions.Options;

namespace PetroineosIntradayReport.Service
{
    public class WindowsService : BackgroundService
    {
        private readonly ILogger<WindowsService> _logger;
        private readonly IOptions<Configuration> _configuration;
        private readonly IntradayReportService _intradayReportService;

        public WindowsService(ILogger<WindowsService> logger, IOptions<Configuration> configuration, IntradayReportService intradayReportService)
        {
            _logger = logger;
            _configuration = configuration;
            _intradayReportService = intradayReportService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                _logger.LogInformation("Started");
                while (!stoppingToken.IsCancellationRequested)
                {
                    _intradayReportService.GenerateIntradayReport();
                    await Task.Delay(TimeSpan.FromMinutes(_configuration.Value.IntervalMinutes), stoppingToken);
                }
                _logger.LogInformation("Stopped");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                Environment.Exit(1);
            }

        }
    }
}